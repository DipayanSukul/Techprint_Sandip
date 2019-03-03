$(document).ready(function() {
    $('.divgrid table td').hover(function() {
        $('.divgrid table tr').each(function() {
            $(this).removeClass('tdonhover');
        });
        $(this).parent().addClass('tdonhover');
    });
});