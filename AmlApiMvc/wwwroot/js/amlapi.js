$(document).ready(function () {
    // Function to populate the selector with options fetched from the MVC endpoint
    function populateOptions() {
        $.ajax({
            url: '/Aml/GetNetworkTypes', // Replace with the actual endpoint URL
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                // Clear existing options
                $('#selectOption').empty();

                // Populate options from the data received
                $.each(data, function (index, option) {
                    $('#selectOption').append('<option value="' + option.value + '">' + option.text + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.log('Error fetching options:', error);
            }
        });
    }

    // Function to perform form validation before submission
    function validateForm() {
        var selectValue = $('#selectOption').val();
        var inputValue = $('#stringInput').val();

        // Clear previous error messages
        $('#selectOptionError').text('');
        $('#stringInputError').text('');
        
        // Check if the select and input fields are filled
        var valid = true;
        if (!selectValue) {
            $('#selectOption').css('border', '1px solid red');
            $('#selectOptionError').text('Please choose an option.');
            valid = false;
        } else {
            $('#selectOption').css('border', '1px solid #ccc');
        }
        if (!inputValue) {
            $('#stringInput').css('border', '1px solid red');
            $('#stringInputError').text('Please enter a string.');
            valid = false;
        } else {
            $('#stringInput').css('border', '1px solid #ccc');
        }

        // Additional validation rules can be added here
        // For example, you can check if the input follows specific format requirements

        // If the form passes validation, allow form submission
        return valid;
    }

    // Call the function to populate options when the page is loaded
    populateOptions();

    // Attach form submission event handler
    $('#myForm').submit(function (event) {
        if (!validateForm()) {
            event.preventDefault(); // Prevent form submission if validation fails
        }
    });
});
