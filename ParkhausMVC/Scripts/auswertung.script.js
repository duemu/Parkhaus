
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

$(document).ready(function () {

    $('.log-datetimepicker').datetimepicker({
        locale: 'de',
            showClear: true
        
    });

$('#log-von-datum').on('dp.change', function (e) {
    console.log(e);
    var date = $("#log-bis-datum").val();
    $("#log-bis-datum").data("DateTimePicker").minDate(e.date);
    $("#log-bis-datum").val(date);
});

$('#log-bis-datum').on('dp.change', function (e) {
    console.log(e);
    var date = $("#log-von-datum").val();
    $("#log-von-datum").data("DateTimePicker").maxDate(e.date);
    $("#log-von-datum").val(date);
});

$('#btn-log-filter').click(function () {


    var href = "?1=1";
    var von_datum = $('#log-von-datum').data('date');

    if (von_datum) href += "&von_datum_string=" + moment(von_datum, "DD.MM.YYYY HH:mm").format('DDMMYYYYHHmm');

    var bis_datum = $('#log-bis-datum').data('date');
    if (bis_datum) href += "&bis_datum_string=" + moment(bis_datum, "DD.MM.YYYY HH:mm").format('DDMMYYYYHHmm');
    
    var typ = $('.btn-group-typ').find('input:checked').first().val();
    if (typ != "Alle") href += "&typ=" + typ;
  
    document.location.href = '/Auswertung/Protokollierung/'+ href;



})
})