
$("#modal").on("submit", ".formCliente", function (e) {
    
    e.preventDefault();
    const submitButton = $(document.activeElement);

    const iElemet = submitButton.find("i");
    iElemet.hide();

    submitButton.find("div").attr("hidden",false)

    $.ajax({
        url: $(this).attr('action'),
        type: "POST",
        data: $(this).serialize(),
        success: function (response) {

            submitButton.find("div").attr("hidden", true)
            //submitButton.prepend("<i>");
            //submitButton.find("i").addClass("fa-solid fa-check");
            iElemet.show();


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
                $(location).attr('href', finder.getAppFile('Cliente/Index'));
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


