
ci2PILaserNotificacion = function () { };

ci2PILaserNotificacion.Error = function (message) {
    $('#DivCi2PILaserNotificaciones').html('<div class="alert alert-danger alert-dismissable fade in" id="id"> <a class="close" aria-label="close" href="#" data-dismiss="alert">×</a>' + message + '</div>');
};

ci2PILaserNotificacion.Exito = function (message) {
    $('#DivCi2PILaserNotificaciones').html('<div class="alert alert-success alert-dismissable fade in" id="id"> <a class="close" aria-label="close" href="#" data-dismiss="alert">×</a>' + message + '</div>');
};

var mensajesPopUpPorDectecto = {
    mensajeDeExito: "La operación fue realizada de forma exitosa.",
    mensajeDeErrorEsperado: "No fue posible realizar la operación",
    mensajeDeErrorInesperado: "Ocurrió un error al realizar la operación. Si el problema persiste comuníquese con el administrador."
};

(function ($) {

    /*
    https://stackoverflow.com/questions/2086287/how-to-clear-jquery-validation-error-messages
    */
    $.fn.clearValidation = function () {
        //Internal $.validator is exposed through $(form).validate()
        var validator = $(this).validate();
        //Iterate through named elements inside of the form, and mark them as error free
        $('[name]', this).each(function (indece, valor) {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present                 
        });

        /*
        https://stackoverflow.com/questions/169506/obtain-form-input-fields-using-jquery
        */
        var camposDeEntradas = $(this).find('input');
        $.each(camposDeEntradas, function (indece, valor) {
            var campoDeEntrada = $(this);
            /*
            https://stackoverflow.com/questions/263232/determine-if-an-element-has-a-css-class-with-jquery
            */
            if (campoDeEntrada.hasClass("input-validation-error")) {
                campoDeEntrada.removeClass('input-validation-error');
            }
        });

        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }

    //Tomado de https://plugins.jquery.com/serializeObject/ (Version 2.0.3)
    $.fn.serializeObject = function () {
        "use strict";

        var result = {};
        var extend = function (i, element) {
            var node = result[element.name];

            // If node with same name exists already, need to convert it to an array as it
            // is a multi-value field (i.e., checkboxes)

            if ('undefined' !== typeof node && node !== null) {
                if ($.isArray(node)) {
                    node.push(element.value);
                } else {
                    result[element.name] = [node, element.value];
                }
            } else {
                result[element.name] = element.value;
            }
        };

        $.each(this.serializeArray(), extend);
        return result;
    };
})(jQuery);

function generarPopop(idDivDialogo, idForm, idBoton, url, token) {
    generarPopop(idDivDialogo, idForm, idBoton, url, token, null);
}

function generarPopop(idDivDialogo, idForm, idBoton, url, token, mensajesPopUp) {
    generarPopop(idDivDialogo, idForm, idBoton, url, token, mensajesPopUp, null);
}

function generarPopop(idDivDialogo, idForm, idBoton, url, token, mensajesPopUp, funcionDeExito) {
    if (mensajesPopUp == null) {
        mensajesPopUp = mensajesPopUpPorDectecto;
    }

    $('#' + idDivDialogo).dialog({
        autoOpen: false, width: 400, resizable: false, modal: true,
        buttons: {
            "Continuar": function () {
                var form = $("#" + idForm);
                if (form.valid()) {
                    var formData = form.serializeObject();
                    formData.__RequestVerificationToken = token;

                    $.ajax({
                        type: "POST",
                        url: url,
                        data: formData,
                        dataType: "json",
                        success: function (result) {
                            if (!result.LLamadoExitoso) {
                                if (result.HayCamposInvalidos) {
                                    ActualizarEstadoDeControles(result);
                                    $('#' + idDivDialogo).dialog('open');
                                } else {
                                    alert(mensajesPopUp.mensajeDeErrorEsperado);
                                }
                            } else {
                                alert(mensajesPopUp.mensajeDeExito);
                                // Make sure the callback is a function​
                                if (typeof funcionDeExito === "function") {
                                    // Call it, since we have confirmed it is callable​
                                    funcionDeExito(result);
                                }

                            }
                        },
                        error: function (result) {
                            alert(mensajesPopUp.mensajeDeErrorInesperado)
                        }
                    });

                    $(this).dialog("close");
                }
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#' + idBoton).click(function (evt) {
        evt.preventDefault();

        //Limpiar el contenido de los campos en el formulario
        document.getElementById(idForm).reset();

        $("#" + idForm).clearValidation();

        $('#' + idDivDialogo).dialog('open');

    });
}