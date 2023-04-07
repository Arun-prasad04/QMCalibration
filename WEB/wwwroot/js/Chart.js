function generateChart() {
    var data = {
      DepartmentId: $('#DepartmentId').val(),
      InstrumentType: $('#InstrumentType').val(),
    }
    var labels = [];
    var values = [];
  
    $.ajax({
      url: '../InstrumentReport/InstrumentPieChart',
      type: 'POST',
      data: { instrumentpiechart: data },
      datatype: "json"
    }).done(function (resultObject) {
      for (let item of resultObject) {
        labels.push(item.label);
        values.push(item.value);
      }
      if (resultObject != 0) {
        UpdatepieChart(labels, values)
      }
      else {
        
        Swal.fire({
          icon: 'warning',
          title: 'Warning',
          text: "There is no Instrument",
          footer: '',
          showClass: {
            popup: 'animate__animated animate__fadeInDown'
          },
          hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
          }
        });
        window.location.reload();
      }
  
    });
  }
  
  function UpdatepieChart(labels, values) {
    console.log(labels, values, 'test values...')
    const colorcodes = [];
    if(labels !== null && labels && values !== null && values){
      values.forEach((element)=>{
        const randomColor = Math.floor(Math.random() * 16777215).toString(16);
        var hexcolor = "#"+ randomColor;
        colorcodes.push(hexcolor)
      });
    
    
      var data = {
        datasets: [{
          data: values,
          backgroundColor: colorcodes,
          label: 'Instrument '
        }],
        labels: labels
      };
      var pieOptions = {
        animation: {
          duration: 500,
          easing: "easeOutQuart",
          onComplete: function () {
            var ctx = this.chart.ctx;
            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
            ctx.textAlign = 'center';
            ctx.textBaseline = 'bottom';
            this.data.datasets.forEach(function (dataset) {
              for (var i = 0; i < dataset.data.length; i++) {
                var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
                  total = dataset._meta[Object.keys(dataset._meta)[0]].total,
                  mid_radius = model.innerRadius + (model.outerRadius - model.innerRadius) / 2,
                  start_angle = model.startAngle,
                  end_angle = model.endAngle,
                  mid_angle = start_angle + (end_angle - start_angle) / 2;
                var x = mid_radius * Math.cos(mid_angle);
                var y = mid_radius * Math.sin(mid_angle);
                ctx.fillStyle = '#fff';
                if (i == 3) { // Darker text color for lighter background
                  ctx.fillStyle = '#444';
                }
                var percent = String(Math.round(dataset.data[i] / total * 100)) + "%";
                ctx.fillText(dataset.data[i], model.x + x, model.y + y);
                // Display percent in another line, line break doesn't work for fillText
                //ctx.fillText(percent, model.x + x, model.y + y + 15);
              }
            });
          }
        }
      };
      var pieChartCanvas = $("#pieChart");
      var pieChart = new Chart(pieChartCanvas, {
        type: 'pie', // or doughnut
        data: data,
        options: pieOptions
      });
    }
    else {
      console.log("Failed")
    }
  }

  function showDataWarning(warningMsg) {
  }