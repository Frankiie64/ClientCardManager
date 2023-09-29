$(document).ready(function () {

    const table = $("#tablaTipoTarjeta");
    const topDivTable = $("TopDivTable")

    $("#btnCrear").click(function () {
        $("#modalTarjeta").load("/TipoTarjeta/Crear", function () {
            $("#TipoTarjetaModal").modal("show");
        });
    });

    $("#tablaTipoTarjeta").on("click", ".btnEditar", function (e) {
        $("#modalTarjeta").load("/TipoTarjeta/Editar/" + $(this).attr("asp-data-id"), function () {
            $("#TipoTarjetaModal").modal("show");
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
                search: "Buscar Tarjeta:",
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
                "url": finder.getAppFile("TipoTarjeta/ObtenerTipoTarjetas"),
                "type": "get",
                "datatype": "json",
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
                { "data": "id", },
                { "data": "nombre", },
                { "data": "tarjetas" },
                { "data": "ultimaModificacion" },
                {
                    data: "id",                    
                    render: function (data, type, row, meta) {

                        return `<button  title="Editar TipoTarjeta" type="button" class="btn btn-warning text-light col-4 btnEditar" asp-data-id="${row['id']}" ><i class="fa-solid fa-pencil"></i> Editar</button>
`;
                    }
                }                
            ],           
            "order": [[0, 'asc']]
        });

        $("#tablaTipoTarjeta_wrapper").prepend($("<div>").addClass("row mb-4").attr("id", "top_row"));
        $("#top_row").prepend($("#tablaTipoTarjeta_filter").addClass("col-6"));
        $("#top_row").prepend($("#TopDivTable"));
    }

    cargarTabla();

});




