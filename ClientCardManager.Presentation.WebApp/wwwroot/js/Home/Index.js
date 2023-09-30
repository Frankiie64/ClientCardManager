$(document).ready(function () {
    const ctx = document.getElementById('myChart');
    const spiner = document.getElementById('spiner')

    
    fetch(finder.getAppFile("TipoTarjeta/ObtenerTipoTarjetas"))
        .then(response => response.json())
        .then(data => {

            spiner.classList.remove("spinner-border");
            const labels = [];
            const values = [];

            data.data.forEach(item => {
                labels.push(item.nombre);
                values.push(item.tarjetas);
            });
    
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Tipo de Tarjetas',
                        data: values,
                        borderWidth: 1,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.6)',
                            'rgba(54, 162, 235, 0.6)',
                            'rgba(255, 206, 86, 0.6)',
                            'rgba(75, 192, 192, 0.6)',    
                            'rgba(153, 102, 255, 0.6)',  
                            'rgba(255, 159, 64, 0.6)',   
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',      
                            'rgba(153, 102, 255, 1)',    
                            'rgba(255, 159, 64, 1)',     
                        ],
                        hoverBackgroundColor: [
                            'rgba(255, 99, 132, 0.8)',
                            'rgba(54, 162, 235, 0.8)',
                            'rgba(255, 206, 86, 0.8)',
                            'rgba(75, 192, 192, 0.8)',    
                            'rgba(153, 102, 255, 0.8)',  
                            'rgba(255, 159, 64, 0.8)',   
                        ],
                        hoverBorderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',      
                            'rgba(153, 102, 255, 1)',    
                            'rgba(255, 159, 64, 1)',     
                        ]
                    }]

                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    },
                    plugins: {
                        legend: {
                            labels: {
                                boxWidth: 0 
                            }
                        }
                    }
                }
            });
        })
        .catch(error => {
            console.error('Error al obtener datos desde el servicio:', error);
        });
});
