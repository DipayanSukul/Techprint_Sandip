function checkDel() {
    alert($('#ctl00_ContentPlaceHolder1_lblComapnyID').text());
    if ($('#ctl00_ContentPlaceHolder1_lblComapnyID').text() == '') {
        alert('No Company Found');
    }
    else {
        alert('Do you want to delete');
    }
    
}