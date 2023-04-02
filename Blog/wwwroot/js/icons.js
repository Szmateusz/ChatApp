
function togglePassword() {

    let element = document.getElementById("Password");
    let img = document.getElementById("img");

    if (element.type == "password") {
        element.type = "text";
        img.src = "/lib/icons/visible.png";
    } else {
        element.type = "password";
        img.src = "/lib/icons/unvisible.png";

    }

    
}