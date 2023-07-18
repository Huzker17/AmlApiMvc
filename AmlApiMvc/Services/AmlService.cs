using AmlApiMvc.Data;
using AmlApiMvc.Interfaces;
using AmlApiMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace AmlApiMvc.Services
{
    public class AmlService : IAmlService
    {
        private const string AmlEndpoint = "https://extrnlapiendpoint.silencatech.com/";
        private const string AmlReCheckEndpoint = "https://extrnlapiendpoint.silencatech.com/recheck";
        private const string AccessKey = "";
        private readonly RestClient _restClient;
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public AmlService(ApplicationDbContext db, IMemoryCache cache)
        {
            _restClient = new RestClient();
            _restClient.AddDefaultHeader("Authorization", $"Bearer {AccessKey}");
            _cache = cache;
            _db = db;
        }

        /// <summary>
        /// Отправляем запрос в Aml-API
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<AmlResponse> SendToAmlAsync(WalletAddress walletAddress)
        {
            if (walletAddress == null)
                throw new ArgumentNullException(nameof(walletAddress));

            string token = CreateMD5($"{walletAddress.Address}:{AccessKey}:{string.Empty}");
            var amlRequest = new AmlRequest("", "", walletAddress.Address,walletAddress.NetworkType, token);
            await _db.AmlRequests.AddAsync(amlRequest);
            await _db.WalletAddresses.AddAsync(walletAddress);

            string requestContent = JsonConvert.SerializeObject(amlRequest);
            var restRequest = new RestRequest(requestContent)
            {
                Resource = AmlEndpoint
            };
            var response = await _restClient.PostAsync(restRequest);

            if (response.IsSuccessStatusCode)
            {
                var amlResponse = await HandleResponseAsync(response);
                await _db.AmlResponses.AddAsync(amlResponse);
                await _db.SaveChangesAsync();
                return amlResponse;
            }
            throw new HttpRequestException("Error occurred while sending request to Aml Api", null, response.StatusCode);
        }

        /// <summary>
        /// Переотправляем запрос в Aml-API
        /// </summary>
        /// <param name="reCheckRequest"></param>
        /// <returns>AmlResponse</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<AmlResponse> ResendToAmlApi(AmlReCheckRequest reCheckRequest)
        {
            if (reCheckRequest == null)
                throw new NullReferenceException(nameof(reCheckRequest));

            await _db.AmlReCheckRequests.AddAsync(reCheckRequest);
            string requestContent = JsonConvert.SerializeObject(reCheckRequest);

            var restRequest = new RestRequest(requestContent)
            {
                Resource = AmlReCheckEndpoint
            };
            var response = await _restClient.PostAsync(restRequest);
            if (response.IsSuccessStatusCode)
            {
                var amlResponse = await HandleResponseAsync(response);
                await _db.AmlResponses.AddAsync(amlResponse);
                await _db.SaveChangesAsync();
                return await HandleResponseAsync(response);
            }

            throw new HttpRequestException("Error occurred while sending request to Aml Api", null, response.StatusCode);
        }


        /// <summary>
        /// Обработка ответа от Aml-Api
        /// </summary>
        /// <param name="message"></param>
        /// <returns>AmlResponse</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private async Task<AmlResponse> HandleResponseAsync(RestResponse message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var content = message.Content ?? throw new NullReferenceException(nameof(message.Content));

            var amlResponse = JsonConvert.DeserializeObject<AmlResponse>(content) ?? throw new NullReferenceException(nameof(content));

            if (CheckForResend(amlResponse))
            {
                var token = CreateMD5($"{amlResponse.Uid}:{AccessKey}:{string.Empty}");
                var amlReCheckRequest = new AmlReCheckRequest("", amlResponse.Uid, token);
                return await ResendToAmlApi(amlReCheckRequest);
            }

            return amlResponse ?? throw new NullReferenceException(nameof(amlResponse));
        }
        /// <summary>
        /// Получаем типы сетей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NetworkType>> GetNetworkTypesAsync()
        {
            if (_cache.TryGetValue("networkTypes", out IEnumerable<NetworkType>? result))
            {
                if (result == null)
                {
                    result = await _db.NetworkTypes.ToArrayAsync();
                    _cache.Set("networkTypes", result);
                    return result;
                }

                return result;
            }
            return await _db.NetworkTypes.ToArrayAsync();
        }

        /// <summary>
        /// Создаём токен на основе входных параметров
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        /// <summary>
        /// Проверяем статус ответа на то, что требуется ли переотправка
        /// </summary>
        /// <param name="amlResponse"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static bool CheckForResend(AmlResponse amlResponse)
        {
            if (amlResponse == null)
                throw new ArgumentNullException(nameof(amlResponse));

            if (amlResponse.Status == AmlResponseStatus.Pending)
                return true;

            return false;
        }
    }
}
