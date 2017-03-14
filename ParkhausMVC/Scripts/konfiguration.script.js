
//Event Listener für die Stockwerkkonfiguration
$('#tbl-stockwerk').on('click', '.button', function () {
    //Holt den namen des Feldes -> Funktion
    var name = $(this).data('name');
    //Holt die zeile
    var $tr = $(this).parents("tr");
	var stockwerkID = $tr.data("stockwerkid");

	switch (name) {
	    case "edit":
	        $tr.addClass("edit");
	        $tr.find("input").prop("readOnly", false);
	        $(this).hide();
	        $tr.find(".button[data-name=save]").show();
			break;
		case "add":
		    $.post("/Konfiguration/stockwerk_hinzufuegen", {
				stockwerkBezeichnung: $tr.find("input[name=txt-bezeichnung]").first().val(),
				anzParkplaetze: $tr.find("input[name=txt-anzahl]").first().val()
			}).done(function (data) {
			    location.reload();
			});
			break;
	    case "save":
	        $(this).hide();
	        $tr.removeClass("edit");
	        $tr.find("input").prop("readOnly", true);
	        $tr.find(".button[data-name=edit]").show();
	        
	        console.log(stockwerkID);
	        $.post("/konfiguration/stockwerk_bearbeiten", {
	            id: stockwerkID,
	            bezeichnung:    $tr.find("input[name=txt-bezeichnung]").first().val(),
	            anzParkplaetze: $tr.find("input[name=txt-anzahl]").first().val()
	        }).done(function (data) {
	            if (data.success == false) {
	                $('#stockwerk-alert .message').text(data.error);
	                $('#stockwerk-alert').show();
	            }

	        });

	        break;
	    case "remove":
	        $.post("/konfiguration/stockwerk_loeschen", {
	            id: stockwerkID
	        }).done(function (data) {
	            if (data.success == false) {
	                $('#stockwerk-alert .message').text(data.error);
	                $('#stockwerk-alert').show();
	            }else{
	                $tr.hide();
	            }
	        });
			
			break;
		case "clear":
			$tr.find('input').val("");
			break;
	}
})


$('.tbl-tarif').on('click', '.button', function () {
    var name = $(this).data('name');
    var $tr = $(this).parents("tr");
    var tarifID = $tr.data("tarifid");

    var input_zeit = $tr.find("input[name=txt-zeit]").first().val();

    if (input_zeit == null) input_zeit = 0;

    switch (name) {
        case "edit":
            $tr.addClass("edit");
            $tr.find("input").prop("readOnly", false);
            $(this).hide();
            $tr.find(".button[data-name=save]").show();
            break;
        case "add":
            $.post("/Konfiguration/tarif_hinzufuegen", {
                typ:  $tr.data("typ"),
                zeit: input_zeit,
                preis: $tr.find("input[name=txt-preis]").first().val()
            }).done(function (data) {
                location.reload();
            });
            break;
        case "save":
            $(this).hide();
            $tr.removeClass("edit");
            $tr.find("input").prop("readOnly", true);
            $tr.find(".button[data-name=edit]").show();

            $.post("/konfiguration/tarif_bearbeiten", {
                id: tarifID,
                zeit: input_zeit,
                preis: $tr.find("input[name=txt-preis]").first().val()
            }).done(function (data) {
                console.log(data);
            });

            break;
        case "remove":
            $.post("/konfiguration/tarif_loeschen", {
                id: tarifID
            }).done(function (data) {
                $tr.hide();
            });

            break;
        case "clear":
            $tr.find('input').val("");
            break;
    }
})


