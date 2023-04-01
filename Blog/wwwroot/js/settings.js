
function DeleteTr(id) {

    var element = document.getElementById(id);
 
    var parent = element.parentNode;

    parent.removeChild(element);
}