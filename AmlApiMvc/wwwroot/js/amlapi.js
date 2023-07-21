const itemsPerPage = 8;
$(document).ready(function () {

    fillNetworkTypes();
    fillAmlResponses();

});
function fillAmlResponses() {
    $.ajax({
        url: '/Aml/GetAmlResponses',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#networkType').empty();
            displayPage(1, response);
            generatePagination(response);
        },
        error: function (xhr, status, error) {
            console.log('Error fetching options:', error);
        }
    });
}
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
        $('#walletAddressError').text('������� �����.');
        valid = false;
    } else {
        $('#walletAddress').css('border', '1px solid #ccc');
    }

    return valid;
}

function sendWalletAddress() {
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

    $.ajax({
        url: '/Aml/SendWalletAddress',
        type: 'POST',
        data: JSON.stringify(formData),
        contentType: 'application/json',
        success: function (response) {
            checkAmlResponse(response)
        },
        error: function (xhr, status, error) {
            showPopup("Ошибка", "Возникла ошибка формата адреса: " + "'" +address+"'"+ "should be valid DOGE address or script", "Error")
        }
    });
}
function checkAmlResponse(response) {
    if (response == "Warning")
    {
        showPopup("Внимание!", "Вероятно по данному адресу ещё нет"+ 
            "информации в базе данных. Рекомендуем проверить через" +
            "несколько минут после подтверждения первой транзакции для этого адреса", response);
    }
}

// Function to display a specific page of items
function displayPage(pageNumber, response) {
    const startIndex = (pageNumber - 1) * itemsPerPage;
    const endIndex = Math.min(startIndex + itemsPerPage, response.length);

    const addressList = $('#addressList');
    addressList.empty();

    for (let i = startIndex; i < endIndex; i++) {
        const address = response[i];
        const listItem = $('<li></li>');
        listItem.text(address.dateTime + ' ' + address.address + ' ' + address.network + ' ' + address.riskScore);
        if (i === response.length - 1) {
            listItem.append(' - <a href="' + address.xlsReport + '">Скачать</a>');
        }
        addressList.append(listItem);
    }
}

// Function to generate pagination links
function generatePagination(response) {
    const totalPages = Math.ceil(response.length / itemsPerPage);
    const pagination = $('#pagination');
    pagination.empty();

    for (let i = 1; i <= totalPages; i++) {
        const pageLink = $('<a href="#" data-page="' + i + '">' + i + '</a>');
        pageLink.on('click', function () {
            const pageNumber = $(this).data('page');
            displayPage(pageNumber);
        });
        pagination.append(pageLink);
    }
}


function showPopup(headerMessage, bodyMessage, typeOfPopup) {
    const buttonContainer = document.getElementById('popupButtonContainer');
    if (typeOfPopup == "Warning") {
        document.getElementById('popup').style.backgroundColor = '#302a75';

        const checkButton = document.createElement("button");
        checkButton.textContent = "���������";
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





