$(document).ready(function () {

    //Datapicker initialisieren
    $(function () {
        $('.datetimepicker').datetimepicker({
        
        });
    });

    //Wird beim dem Radiobutton der Dauermieter ausgewählt, wird der Eintrittscode aktiviert
    $("input[type=radio][name=parking-typ]").change(function () {
        if ($("#rdoDauermieter").prop("checked")) {
            $('#eintrittscode').prop('disabled', false);
        } else {
            $('#eintrittscode').prop('disabled', true);
        }
    });

    //Wenn ein Eingangsdatum eingegeben wurde, wird der "Eintreten"-Button aktiviert
    $('#eintrittsdatum').on('dp.hide', function () {
        if (!$(this).val().length) {
            $('#btnEintreten').prop('disabled', true);
        } else {
            $('#btnEintreten').prop('disabled', false);
        }
    })

    //Wenn ein Austrittsdatum eingegeben wurde, wird der "Austreten"-Button aktiviert
    $('#austrittsdatum').on('dp.hide', function () {
       
        if (!$(this).val().length || !$('.pp.selected').length) {
            $('#btnAustreten').prop('disabled', true);
        } else {
            $('#btnAustreten').prop('disabled', false);
        }
    })


    //Event Listener für den Eintrittsbutton Click
    $("#btnEintreten").click(function () {
        //Holt den Eintrittscode
        var code_val = $('#eintrittscode').val();
        if (!code_val) code_val = 0;
        $.ajax({
            url: '/home/eintreten', //Ruft die eintreten-Methode auf dem HomeController auf
            type: 'POST',
            data: { //Übergibt die Daten
                eintritttsdatum: $('#eintrittsdatum').val(),
                typ: $('input[type=radio][name=parking-typ]:checked').val(),
                code: code_val
            },
            success: function (data) {
                if (!data.success) {
                    //Zeigt eine Fehlermeldung an, wenn eine Exception geworfen wird
                    $('#steuerung-alert .message').text(data.error);
                    $('#steuerung-alert').show();
                }
                else { //Request war erfolgreich
                    //Versteckt die Fehlermeldung
                    $('#steuerung-alert').hide();
                    //Setzt das Eintrittsdatum zurück
                    $('#eintrittsdatum').val("");
                    //Setzt das Eingangscode zurück
                    $('#eintrittscode').val("");
                    //Deaktiviert den "Eintreten"-Button
                    $('#btnEintreten').prop('disabled', true);

                    //Berechnet die position des Stockwerkts 
                    var elementTop = $('#stockwerk' + data.stockwerkID).offset().top;
                    var divTop = $('.pp-row').first().offset().top;
                    var elementRelativeTop = elementTop - divTop;

                    // Scrollt zu dem aktuellen Stockwerk
                    $('.pp-row').animate({
                        scrollTop: elementRelativeTop
                    }, 500);

                    //Setzt den Parkplatz auf belegt
                    $('#' + data.parkplatzID).addClass("belegt");
                    //Bei dem gelegenheitsnutzer wird das Ticket ausgegeben
                    if (typ = 'Gelegenheitsnutzer') {
                        ticket_anzeigen(data.parkplatzID);
                    }

                }
            }
        });

    });

    //Event Listener für den Austretenbutton Click
    $("#btnAustreten").click(function () {
        //Holt den ausgewählten Parkplatz
        var id =  $('.pp.selected').first().attr('id');
        $.ajax({
            url: '/home/austreten', //Ruft die austreten-Methode auf dem HomeController auf
            type: 'POST',
            data: { //Übergibt die Daten
                austrittsdatum: $('#austrittsdatum').val(),
                parkplatzID: id
            },
            success: function (data) {
                if (!data.success) {
                    //Zeigt eine Fehlermeldung an, wenn eine Exception geworfen wird
                    $('#steuerung-alert .message').text(data.error);
                    $('#steuerung-alert').show();
                }
                else { //Request war erfolgreich
                    
                    //Gibt den Parkplatz frei
                    $('#' + id).removeClass("belegt");
                    $('#' + id).removeClass("selected");
                    $('#' + id).addClass("frei");

                    //Zeigt die Austrittsquittung an
                    $ticket = $("#ticket_div .ticket");
                    $ticket.find(".quittung-element").show();
                    $ticket.find(".ticket-preis").first().html(data.preis);
                    $ticket.find(".ticket-austrittsdatum").first().html($('#austrittsdatum').val());
                    //Setzt das Ausrtittsdatum zurück
                    $('#austrittsdatum').val("");

                }
            }
        });

    });

    //Zeigt das Ticket an
    function ticket_anzeigen(id) {
        $.ajax({
            url: '/home/ticket_anzeigen', //Ruft die ticket_anzeigen-Methode auf dem HomeController auf
            data: { ParkplatzID: id }, //Übergibt die ParkplatzID
            success: function (data) { //Request war erfolgreich
                //Wandelt das Datum in das JavaScript Date Format um 
                var eintritttsdatumzeit = new Date(parseInt(data.eingangsdatum.substr(6)));
                //Unterscheidet zwischen eintrittszeit und eintrittsdatum 
                var eintritttsdatum = moment(eintritttsdatumzeit).format('DD.MMMM.YYYY');
                var eintritttszeit = moment(eintritttsdatumzeit).format('HH:mm');
                //Deselektiert alle Parkplatze
                $('.pp').removeClass("selected");
                //Selektiert den aktuellen Parkplatz
                $('#' + id).addClass('selected');
                //Aktiviert das Austrittsdatum
                $('#austrittsdatum').prop('disabled', false);
                //Setzt die minimalzeit für das Austrittsdatum auf das Eintrittsdatum plus eine Minute
                $('#austrittsdatum').data("DateTimePicker").minDate(moment(eintritttsdatumzeit).add(1, 'm'));

                //Zeigt das Ticket nur bei einem Gelegenheitsnutzer an
                if(!$('#'+id).hasClass("dauermieter")){
                    //Holt den Ticketcontainer
                    $ticket = $("#ticket_div .ticket");
                    //Setzt die die Ticketwerte
                    $ticket.find(".quittung-element").hide();
                    $ticket.find(".ticket-eintrittsdatum").first().text(eintritttsdatum);
                    $ticket.find(".ticket-eintrittszeit").first().text(eintritttszeit);
                    $ticket.find(".ticket-parkplatznr").first().text(data.parkplatzNr);
                    $ticket.find(".ticket-stockwerk").first().text(data.stockwerk);
                    $ticket.find(".ticket-barcode").first().text(data.ticketID);
                    $ticket.find(".ticket-ticketID").first().text(data.ticketID);
                    //Zeigt dast ticket an
                    $("#ticket_div .ticket").show();
                }
            },
            type: 'POST'
        });
    }

    //Event Listener für den Click auf ein belegter Parkplatz
    $(".pp-row").on("click", ".pp.belegt", function () {
       
        //Prüft ob der Parkplatz bereits ausgewählt ist
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");
            $("#ticket_div .ticket").hide();
            $('#btnAustreten').prop('disabled', true);
            $('#austrittsdatum').prop('disabled', true);
            $('#austrittsdatum').val('');
            return false;
        }
        //Zeigt das Ticket an
        ticket_anzeigen(this.id);

    });

    //Event Listener für das ein und ausklappen der Schrankensteuerung
    $("#sidebar-pfeil").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
        $("#sidebar-pfeil").toggleClass("glyphicon-circle-arrow-right");
        $("#sidebar-pfeil").toggleClass("glyphicon-circle-arrow-left");
    });



})