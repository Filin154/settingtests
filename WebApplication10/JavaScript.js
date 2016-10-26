
function EnsureNumeric()
{
    var key = window.event.keyCode; 
    if (key <48 || key >57) 
        window.event.returnValue = false; 
}
