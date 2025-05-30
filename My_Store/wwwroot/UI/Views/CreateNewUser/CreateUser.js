﻿import { UserService } from "../../Services/user.service.js";


document.addEventListener("DOMContentLoaded", function () {

    const inputPassword = document.getElementById("my_store_new_user_password_input");
    const checkboxPassword = document.getElementById("my_store_new_user_checkbox_input");
    const userForm = document.getElementById("userForm");
    const userEmail = document.getElementById("my_store_new_user_email_input");
    const userPassword = document.getElementById("my_store_new_user_password_input");



 
    checkboxPassword.addEventListener("change", function () {
        inputPassword.type = checkboxPassword.checked ? "text" : "password";
    })


    function toValidateFields(event) {

        if ((userEmail.value.trim() == "") || (userPassword.value.trim() == "")) {
            event.preventDefault();
            alert("Please fill all the fields on the form, please");
            return false;
        }

        if ((userPassword.value.trim().length < 8)) {
            event.preventDefault();
            alert("Please, set your Password with 8 characters or more, please");
            return false
        }


        return true;
    }



    userForm.addEventListener("submit", async  function (event) {
        let isValidate=toValidateFields(event);
      
        if (isValidate) {
            const newUser = {
                email: userEmail.value.trim(),
                password: userPassword.value.trim()
            }
            event.preventDefault();

            try {
                const user = await UserService.createUser((email, password));
                console.log("The user was created succesfully");
            }
            catch (error) {
                alert("There was a mistake", error.message);
            }

        }
   
    })

})