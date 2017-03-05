
$(".dropdown-jahr li a").click(function () {
    var selText = $(this).text();
    $(this).parents('.btn-group-jahr').find('.dropdown-toggle .btn-titel').text(selText);
});

$(".dropdown-monat li a").click(function () {
    var selText = $(this).text();
    $(this).parents('.btn-group-monat').find('.dropdown-toggle .btn-titel').text(selText);
});

$('#btn-filter').click(function () {

    var href = "?1=1";
    var jahr = $('.btn-group-jahr').find('.dropdown-toggle .btn-titel').first().text();
    if (jahr != "Alle Jahre") href += "&jahr=" + jahr;

    var monat = $('.btn-group-monat').find('.dropdown-toggle .btn-titel').first().text();
    if (monat != "Alle Monate") href += "&monat=" + monat;


    var typ = $('.btn-group-typ').find('input:checked').first().val();
    if (typ != "Alle") href += "&typ=" + typ;

    document.location.href = href;



})