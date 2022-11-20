$(document).on( "click", ".common_sender", function(e) {
	e.preventDefault();

	var butt_id = $(this).attr('id') ;
	var progress_bar_html = '<div class="loader" id="inf_loader"><div class="loaderBar"></div></div>';
	
	if (butt_id == "ebulten_submit") {				// frontend kisminda tiklanan BUTTON ebulten icin ise
	// if (butt_id == "widget-subscribe-submit-button") {				// frontend kisminda tiklanan BUTTON ebulten icin ise
		var inp = $("#ebulten_input_addr").val();
		var lang = $("#ebulten_lang").val();

    	var htmlOut = '';

		if ( isValidEmail(inp) ) {

			$(progress_bar_html).insertBefore($(this));
			
 		  $.ajax({
		    url: "/ajx_01.php",
		    type: 'POST',
		    data: { act: 'add_to_ebulten', frm_email: inp, lang: lang },
		    success: function (srvResp) {

				$("#inf_loader").remove();
				
		    	if ( srvResp['sta'] == "OK" ) {
					htmlOut = srvResp['srvSays'];
					// htmlOut = '<?=$in_tagid['con_7']?>';
		    	} else if ( srvResp['sta'] == "ERR" ) {
					htmlOut = srvResp['srvSays'];
					// htmlOut = "Belirtmiş olduğunuz e-posta adresi hatalı. Lütfen geçerli bir e-posta adresi belirtiniz.";
		    		// $("#stg_recover_passwd").html( srvResp['srvSays'] );		    		
		    	} else {
		    		// $("#recover_passwd_returns").html( srvResp['srvSays'] );		
					htmlOut = 'undefined error2';

		    	}

				$("#ebulten_submit").hide();
				// $("#ebulten_submit").fadeOut();
				$("#ebulten_input_addr").prop('readonly', true);
				$("#ebulten_input_addr").css('border', '0px');
				// $("#ebulten_input_addr").addClass('alert');
				// $("#ebulten_input_addr").addClass('alert-warning');

				$("#ebulten_input_addr").val(htmlOut);


				// $("#ebulten_response").hide();				
				// $("#ebulten_response").html(htmlOut);
				// $("#ebulten_response").fadeIn();


		    },
		    error: function (request, status, error) {
		                    console.log("REQUEST:\t" + request + "\nSTATUS:\t" + status + "\nERROR:\t" + error);
		            }
		  });



		} else {
			htmlOut = "Belirtmiş olduğunuz e-posta adresi hatalı. Lütfen geçerli bir e-posta adresi belirtiniz.";
		}
		$("#ebulten_response").hide();
		$("#ebulten_response").html(htmlOut);
		$("#ebulten_response").fadeIn();
	
	} else if (butt_id == "iletisim_submit") {				// frontend kisminda tiklanan BUTTON iletisim icin ise
		var msg_email = $("#msg_email").val();
		var msg_name = $("#msg_name").val();
		var msg_subject = $("#msg_subject").val();
		var msg_body = $("#msg_body").val();

	console.log(msg_email);
		// 
		if ( isValidEmail(msg_email) ) {
		  $(progress_bar_html).insertBefore($(this));
			
 		  $.ajax({
		    url: "/ajx_01.php",
		    type: 'POST',
		    data: { act: 'iletisim_submit', msg_email: msg_email, msg_name: msg_name, msg_subject: msg_subject, msg_body: msg_body},
		    success: function (srvResp) {

				$("#inf_loader").remove();
				
		    	if ( srvResp['sta'] == "OK" ) {
					htmlOut = srvResp['srvSays'];
		    	} else if ( srvResp['sta'] == "ERR" ) {
					htmlOut = srvResp['srvSays'];
					// htmlOut = "Belirtmiş olduğunuz e-posta adresi hatalı. Lütfen geçerli bir e-posta adresi belirtiniz.";
		    		// $("#stg_recover_passwd").html( srvResp['srvSays'] );		    		
		    	} else {
		    		// $("#recover_passwd_returns").html( srvResp['srvSays'] );		    		
		    	}

				$("#iletisim_response").hide();
				$("#iletisim_response").html(htmlOut);
				$("#iletisim_response").fadeIn();

		    },
		    error: function (request, status, error) {
		                    console.log("REQUEST:\t" + request + "\nSTATUS:\t" + status + "\nERROR:\t" + error);
		            }
		  });
 		}

	} else {
		console.log("undefined id");	
	}
});

function isValidEmail(email) {
  var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
  return regex.test(email);
}