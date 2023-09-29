$(document).ready(function () {

    const table = $("#tablaCliente");

    $("#modal").on("input", ".telefono", function (e) {
        $(this).mask('000-000-0000');
    });



    $("#btnCrear").click(function () {
        $("#modal").load("/Cliente/Crear", function () {
            $("#ClienteModal").modal("show");
        });
    });

    $("#tablaCliente").on("click", ".btnEditar", function (e) {
        $("#modal").load("/Cliente/Editar/" + $(this).attr("asp-data-id"), function () {
            $("#ClienteModal").modal("show");
        });
    });

    $("#tablaCliente").on("click", ".btnAsociarTarjeta", function (e) {

        let timerInterval;
        Swal.fire({
            title: 'Información!',
            icon: 'info',
            html: 'Sera redireccionado en <b></b> segundos.<br><button class="btn btn-danger mt-3 me-2" id="cancelar">Cancelar</button><button class="btn btn-info text-light mt-3" id="continuar">Continuar</button>',
            timer: 3000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
                const b = Swal.getHtmlContainer().querySelector('b');
                const cancelarBtn = Swal.getHtmlContainer().querySelector('#cancelar'); 
                const continuarBtn = Swal.getHtmlContainer().querySelector('#continuar'); 

                cancelarBtn.addEventListener('click', () => {
                    Swal.close(); 
                });
                continuarBtn.addEventListener('click', () => {
                    Swal.close();
                    $(location).attr('href', finder.getAppFile('ClienteTarjeta/Index/' + $(this).attr("asp-data-id") ));
                });
                timerInterval = setInterval(() => {
                    b.textContent = Math.ceil(Swal.getTimerLeft() / 1000);
                }, 1000);
            },
            willClose: () => {
                clearInterval(timerInterval);
            }
        }).then((result) => {
            if (result.dismiss === Swal.DismissReason.timer) {
                $(location).attr('href', finder.getAppFile('ClienteTarjeta/Index/' + $(this).attr("asp-data-id")));
            }
        });
    });

   
    function cargarTabla() {

       
        $(table).DataTable({
            bLengthChange: false,
            responsive: true,
            destroy: true,
            scrollX: false,
            pageLength: 5,
            language: {
                processing: "Procesando",
                search: "Buscar Cliente:",
                lengthMenu: "Ver _MENU_ Filas",
                info: "_START_ - _END_ de _TOTAL_ elementos",
                infoEmpty: "0 - 0 de 0 elementos",
                infoFiltered: "(Filtro de _MAX_ entradas en total)",
                infoPostFix: "",
                loadingRecords: "Cargando datos...",
                zeroRecords: "No se encontraron datos",
                emptyTable: "No hay datos disponibles",
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Ultimo"
                },
                aria: {
                    sortAscending: ": activer pour trier la colonne par ordre croissant",
                    sortDescending: ": activer pour trier la colonne par ordre décroissant"
                }
            },
            "ajax": {
                "url": finder.getAppFile("Cliente/ObtenerClientes"),
                "type": "get",
                "datatype": "json",
                //"data": data,
                "async": true,
                error: function (jqXHR, textStatus, errorThrown) {

                    console.error("Error en la solicitud:", textStatus, errorThrown);
                    $(table).DataTable().clear().draw();

                    Swal.fire({
                        icon: 'warning',
                        title: "Atención!!!",
                        text: jqXHR.responseText,
                        showConfirmButton: true,
                        timer: 2500,

                    });

                }
            },
            "columns": [
                { "data": "nombre", },
                { "data": "apellido", },
                { "data": "telefono", },
                { "data": "ocupacion", },      
                { "data": "tarjetas"},
                {
                    data: "id",                    
                    render: function (data, type, row, meta) {

                        let estructura = `<div class='row justify-content-evenly'>
                <button  title="Editar Cliente" type="button" class="btn btn-warning text-light col-4 btnEditar" asp-data-id="${row['id']}" ><i class="fa-solid fa-pencil"></i></button>
                <a " title="Agregar Tarjeta" type="button" class="btn btn-info text-light col-4 btnAsociarTarjeta" asp-data-id="${row['id']}"><i class="fa-solid fa-credit-card"></i></a>
</div>`;
                        return estructura;                      
                    }
                }                
            ],           
            "order": [[0, 'asc']]
        });

        $("#tablaCliente_wrapper").prepend($("<div>").addClass("row mb-4").attr("id", "top_row"));
        $("#top_row").prepend($("#tablaCliente_filter").addClass("col-6"));
        $("#top_row").prepend($("#TopDivTable"));
    }

    cargarTabla();

});




