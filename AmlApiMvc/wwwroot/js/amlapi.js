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
            console.log('Form data sent successfully');
            console.log(response);
        },
        error: function (xhr, status, error) {
            console.log('Error sending form data:', error);
            showErrorPopup("Ошибка", "Возникла ошибка формата адреса: " + "'"+address+"'"+ "should be valid DOGE address or script")
        }
    });
}
function checkAmlResponse(response) {
    if (response == "Error")
    {
        showErrorPopup("Внимание! Вероятно по данному адресус ещё нет"+ 
            "информации в базе данных.Рекомендуем проверить через несколько минут" +
            "после подтверждения первой транзакции для этого адреса.");
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


function showErrorPopup(headerMessage,bodyMessage) {
    document.getElementById('popupHeader').innerText = headerMessage;
    document.getElementById('popupMessage').innerText = bodyMessage;
    document.getElementById('popupContainer').style.display = 'block';
}

document.getElementById('popupCloseButton').addEventListener('click', function () {
    document.getElementById('popupContainer').style.display = 'none';
});
document.get('closePopup1').addEventListener('click', function () {
    document.getElementById('popupContainer').style.display = 'none';
});
document.getElementById('closePopup2').addEventListener('click', function () {
    document.getElementById('popupContainer').style.display = 'none';
});

document.getElementById('popupContainer').addEventListener('click', function (event) {
    if (event.target === this) {
        this.style.display = 'none';
    }
});





