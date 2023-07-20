$(document).ready(function () {
    function fillNetworkTypes() {
        $.ajax({
            url: '/Aml/GetNetworkTypes',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#networkType').empty();
                $.each(data, function (index, option) {
                    $('#networkType').append('<option value="' + option.name + '">' + option.name + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.log('Error fetching options:', error);
            }
        });
    }
    fillNetworkTypes();
});
function validateForm() {
    var selectValue = $('#networkType').val();
    var inputValue = $('#walletAddress').val();

    $('#networkTypeError').text('');
    $('#walletAddressError').text('');

    var valid = true;
    if (!selectValue) {
        $('#networkType').css('border', '1px solid red');
        $('#networkTypeError').text('Please choose an option.');
        valid = false;
    } else {
        $('#networkType').css('border', '1px solid #ccc');
    }
    if (!inputValue) {
        $('#walletAddress').css('border', '1px solid red');
        $('#walletAddressError').text('Введите адрес.');
        valid = false;
    } else {
        $('#walletAddress').css('border', '1px solid #ccc');
    }

    return valid;
}

function sendWalletAddress() {
    console.log("I am here");
    if (!validateForm()) {
        preventDefault();
    }
    $('#walletAddressError').text('');
    $('#networkTypeError').text('');
    var address = $('#walletAddress').val();
    var formData = {
        address: address,
        networkType: $('#networkType').val()
    };
    console.log(formData);

    $.ajax({
        url: '/Aml/SendWalletAddress',
        type: 'POST',
        data: JSON.stringify(formData),
        contentType: 'application/json',
        success: function (response) {
            console.log(response);
            checkAmlResponse(response)
        },
        error: function (xhr, status, error) {
            console.log('Error sending form data:', error);
            showPopup(encodeURI("Ошибка"), encodeURI("Возникла ошибка формата адреса: ") + "'" +address+"'"+ "should be valid DOGE address or script", "Error")
        }
    });
}
function checkAmlResponse(response) {
    if (response == "Warning")
    {
        showPopup(encodeURI("Внимание!"), encodeURI("Вероятно по данному адресус ещё нет"+ 
            "информации в базе данных.Рекомендуем проверить через несколько минут" +
            "после подтверждения первой транзакции для этого адреса."), response);
    }
    else
    {
        var addressList = $('#addressList');
        addressList.empty();
        var header = $('<li></li>');
        listItem.text('Время Адрес  Сеть   Оценка риска Отчет PDF ');
        addressList.append(header);
        $.each(response, function (index, address) {
            var listItem = $('<li></li>');
            listItem.text(address.dateTime + address.address + address.network + address.riskScore );
            if (index === response.length - 1) {
                text += ' - <a href="' + address.xlsReport + '">Скачать</a>';
            }
            addressList.append(listItem);
        });
    }
}

function showPopup(headerMessage, bodyMessage, typeOfPopup) {
    console.log(headerMessage);
    console.log(bodyMessage);
    const buttonContainer = document.getElementById('popupButtonContainer');
    if (typeOfPopup == "Warning") {
        document.getElementById('popup').style.backgroundColor = '#302a75';

        const checkButton = document.createElement("button");
        checkButton.textContent = "Проверить";
        checkButton.id = "closePopup3";
        checkButton.classList.add("closePopup3");
        checkButton.addEventListener('click', function () {
            document.getElementById('popupContainer').style.display = 'none';
        });
        buttonContainer.appendChild(checkButton);
    }
    else {
        document.getElementById('popup').style.backgroundColor = '#ff3a3a';

        const okButton = document.createElement("button");
        okButton.textContent = "OK";
        okButton.id = "closePopup2";
        okButton.addEventListener('click', function () {
            document.getElementById('popupContainer').style.display = 'none';
        });
        okButton.classList.add("closePopup2");

        buttonContainer.appendChild(okButton);
    }
    document.getElementById('popupHeader').innerText = headerMessage;
    document.getElementById('popupMessage').innerText = bodyMessage;
    document.getElementById('popupContainer').style.display = 'block';
}

document.getElementById('popupCloseButton').addEventListener('click', function () {
    document.getElementById('popupContainer').style.display = 'none';
});
document.getElementById('closePopup1').addEventListener('click', function () {
    document.getElementById('popupContainer').style.display = 'none';
});


document.getElementById('popupContainer').addEventListener('click', function (event) {
    if (event.target === this) {
        this.style.display = 'none';
    }
});





