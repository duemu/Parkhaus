$(document).ready(function () {

    //Datapicker initialisieren
    $(function () {
        $('.datetimepicker').datetimepicker({
        
        });
    });

    $("input[type=radio][name=parking-typ]").change(function () {
        if ($("#rdoDauermieter").prop("checked")) {
            $('#eintrittscode').prop('disabled', false);
        } else {
            $('#eintrittscode').prop('disabled', true);
        }

    });

    $('#eintrittsdatum').on('dp.change', function () {
        if (!$(this).val().length) {
            $('#btnEintreten').prop('disabled', true);
        } else {
            $('#btnEintreten').prop('disabled', false);
        }
    })

    $('#austrittsdatum').on('dp.change', function () {
        if (!$(this).val().length) {
            $('#btnAustreten').prop('disabled', true);
        } else {
            $('#btnAustreten').prop('disabled', false);
        }
    })



    $("#btnEintreten").click(function () {
        var code_val = $('#eintrittscode').val();
        if (!code_val) code_val = 0;
        $.ajax({
            url: '/ticket/createTicket',
            type: 'POST',
            data: {
                eintritttsdatum: $('#eintrittsdatum').val(),
                typ: $('input[type=radio][name=parking-typ]:checked').val(),
                code: code_val
            },
            error: function (e) {
                $('#steuerung-alert').show();
            },
            success: function (data) {
                if (!data.success) {
                    $('#steuerung-alert .message').text(data.error);
                    $('#steuerung-alert').show();
                }
                else{
                    $('#eintrittsdatum').val("");
                    $('#eintrittscode').val("");
                    $('#btnEintreten').prop('disabled', true);
                    $('#' + data.parkplatzID).addClass("belegt");
                    if (typ = 'Gelegenheitsnutzer') {
                        ticket_anzeigen(data.parkplatzID);
                    }


                }
            }
        });

    });

    $("#btnAustreten").click(function () {
        var id =  $('.pp.selected').first().attr('id');
        $.ajax({
            url: '/ticket/Austreten',
            type: 'POST',
            data: {
                austrittsdatum: $('#austrittsdatum').val(),
                parkplatzID: id
            },
            success: function (data) {
                if (!data.success) {
                    $('#steuerung-alert .message').text(data.error);
                    $('#steuerung-alert').show();
                }
                else {
                    
                    $('#' + id).removeClass("belegt");
                    $('#' + id).removeClass("selected");
                    $('#' + id).addClass("frei");

                    $ticket = $("#ticket_div .ticket");
                    $ticket.find(".ticket-preis").first().show().html(data.preis);
                    $ticket.find(".ticket-austrittsdatum").first().show().html($('#austrittsdatum').val());
                    
                    $('#austrittsdatum').val("");

                }
            }
        });

    });


    function ticket_anzeigen(id) {
        $.ajax({
            url: '/ticket/getTicket',
            data: { ParkplatzID: id },
     
            success: function (data) {
                console.log(data);
                var eintritttsdatumzeit = new Date(parseInt(data.eingangsdatum.substr(6)));
                var eintritttsdatum = moment(eintritttsdatumzeit).format('DD.MMMM.YYYY');
                var eintritttszeit = moment(eintritttsdatumzeit).format('HH:mm');
                $('.pp').removeClass("selected");
                $('#' + id).addClass('selected');

                $ticket = $("#ticket_div .ticket");
                $ticket.find(".ticket-preis").first().hide();

                $ticket.find(".ticket-austrittsdatum").first().hide();
                $ticket.find(".ticket-eintrittsdatum").first().text(eintritttsdatum);
                $ticket.find(".ticket-eintrittszeit").first().text(eintritttszeit);
                $ticket.find(".ticket-parkplatznr").first().text(data.parkplatzNr);
                $ticket.find(".ticket-stockwerk").first().text(data.stockwerk);
                $ticket.find(".ticket-barcode").first().text(data.ticketID);
                $ticket.find(".ticket-ticketID").first().text(data.ticketID);
                $("#ticket_div .ticket").show();
            },
            type: 'POST'

        });
    }

    $(".pp-row").on("click", ".pp.belegt", function () {
       
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");
            $("#ticket_div .ticket").hide();
            return false;
        }
        var id = this.id;
        $.ajax({
            url: '/ticket/getTicket',
            data: { ParkplatzID: id },
            error: function (e) {
                // console.log(e);
            },
            success: function (data) {
                console.log(data);
                var eintritttsdatumzeit = new Date(parseInt(data.eingangsdatum.substr(6)));
                var eintritttsdatum = moment(eintritttsdatumzeit).format('DD.MMMM.YYYY');
                var eintritttszeit  = moment(eintritttsdatumzeit).format('HH:mm');
                $('.pp').removeClass("selected");
                $('#' + id).addClass('selected');

                $ticket = $("#ticket_div .ticket");
                $ticket.find(".ticket-preis").first().hide();

                $ticket.find(".ticket-austrittsdatum").first().hide();
                $ticket.find(".ticket-eintrittsdatum").first().text(eintritttsdatum);
                $ticket.find(".ticket-eintrittszeit").first().text(eintritttszeit);
                $ticket.find(".ticket-parkplatznr").first().text(data.parkplatzNr);
                $ticket.find(".ticket-stockwerk").first().text(data.stockwerk);
                $ticket.find(".ticket-barcode").first().text(data.ticketID);
                $ticket.find(".ticket-ticketID").first().text(data.ticketID);
                $("#ticket_div .ticket").show();
            },
            type: 'POST'

        });

    });






})