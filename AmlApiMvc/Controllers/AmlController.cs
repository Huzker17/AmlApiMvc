﻿using AmlApiMvc.Interfaces;
using AmlApiMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmlApiMvc.Controllers
{
    public class AmlController : Controller
    {
        private readonly IAmlService _amlService;

        public AmlController(IAmlService amlService)
        {
            _amlService = amlService;
        }
        [HttpPost(Name ="SendWalletAddress")]
        public async Task<IActionResult> SendWalletAddress(WalletAddress walletAddress)
        {
            if (walletAddress == null) 
                throw new ArgumentNullException(nameof(walletAddress));
            
            await _amlService.SendToAmlAsync(walletAddress);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetNetworkTypes()
        {
            var cryptoCurrency = await _amlService.GetNetworkTypesAsync();
            if (cryptoCurrency == null)
                return Ok("There isn't networkTypes");

            return Ok(cryptoCurrency);
        }
    }
}