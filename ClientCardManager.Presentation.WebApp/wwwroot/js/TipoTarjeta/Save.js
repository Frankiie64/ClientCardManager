
const i = "fa-solid fa-check"
const spiner = "spinner-border spinner-border-sm"

$("#modalTarjeta").on("submit", ".formTipoTarjeta", function (e) {

    e.preventDefault();
    const submitButton = $(document.activeElement);

    submitButton.find("i").remove();
    submitButton.prepend("<div>");
    submitButton.find("div").addClass(spiner)

    $.ajax({
        url: $(this).attr('action'),
        type: "POST",
        data: $(this).serialize(),
        success: function (response) {

            submitButton.find("div").remove();
            submitButton.prepend("<i>");
            submitButton.find("i").addClass(i);

            if (!response.ok) {

                Command: toastr['error'](`${response.msj}`, `${response.titulo}`)

                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": true,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
                return;
            }

            Swal.fire({
                icon: 'success',
                title: response.titulo,
                text: response.msj,
                showConfirmButton: true,
                timer: 1800,

            }).then((result) => {
                $(location).attr('href', finder.getAppFile('TipoTarjeta/Index'));
            });

        },
        error: function (error) {
            console.error(error);
            submitButton.find("div").remove();
            submitButton.prepend("<i>");
            submitButton.find("i").addClass(i);

            Command: toastr['error'](`POR FAVOR PONERSE EN CONTACTO CON EL TECNICO.`, `ERROR INTERNO`)

            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    });
    

   
});


