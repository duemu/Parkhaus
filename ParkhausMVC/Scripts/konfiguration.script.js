$('#tbl-stockwerk').on('click', '.button', function () {
	var name = $(this).data('name');
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
				var $newTr = $tr.before().clone(true);
				$tr.before().append($newTr);
				$tr.find('input').val("");
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
	            console.log(data);
	        });

	        break;
	    case "remove":
	        $.post("/konfiguration/stockwerk_loeschen", {
	            id: stockwerkID
	        }).done(function (data) {
	            $tr.hide();
	        });
			
			break;
		case "clear":
			$tr.find('input').val("");
			break;
	}
})

