function generateHourlyForecast(dataPoints, graphTitle, xAxisTitle, yAxisTitle) {
    var chart = new CanvasJS.Chart("chart", {
        animationEnabled: true,
        theme: "light2", // "light1", "light2", "dark1", "dark2"
        title: {
            text: graphTitle
        },
        axisY: {
            title: yAxisTitle
        },
        axisX: {
            title: xAxisTitle
        },
        data: [{
            type: "column",
            dataPoints: dataPoints
        }]
    });
    chart.render();
}