myChart = null;

window.owGraph = {
  clear: function () {
    myChart = null;
  },

  show: function (type, labels, dataSeries, optionsType, min, max) {
    var ctx = document.getElementById('myChart').getContext('2d');
    var datasets = dataSeries.map(p => this.getDataSeriesSettings(p));
    var options = this.generateOptions(optionsType, min, max);
    if (myChart) {
      myChart.destroy();
    }
    myChart = new Chart(ctx, { type, data: { labels, datasets }, options });
  },

  getDataSeriesSettings: function (series) {
    return {
      label: series.label,
      data: series.data,
      borderColor: series.color,
      backgroundColor: series.color,
      pointBackgroundColor: series.color,
      pointBorderColor: series.color,
      pointHoverBackgroundColor: series.color,
      pointHoverBorderColor: series.color,
      fill: false,
      cubicInterpolationMode: 'monotone',
      borderWidth: 3,
      pointRadius: 5,
      lineTension: 0.4
    }
  },

  generateOptions: function (type, min, max) {
    const options = {
      responsive: true,
      maintainAspectRatio: false,
      legend: {
        position: 'top',
        labels: {
          fontSize: 16,
          fontFamily: 'Roboto Condensed',
          padding: 16
        }
      },
      title: {
        display: false
      },
      tooltips: {
        mode: 'point',
        callbacks: {}
      },
      scales:
      {
        xAxes: [{
          display: true,
          scaleLabel:
          {
            display: true
          }
        }],
        yAxes: [
          {
            display: true,
            scaleLabel: {
              display: true,
              labelString: 'Minutes'
            },
            ticks: {
              stepSize: 60
            },
          }
        ]
      }
    };

    var _this = this;

    if (type === 'Time') {
      options.scales.yAxes[0].ticks.suggestedMax = 0;
      options.scales.yAxes[0].ticks.suggestedMin = -180;
      options.scales.yAxes[0].ticks.stepSize = 60;
      options.plugins = {
        tooltip: {
          callbacks: {
            label: function (context) {
              return (context.dataset.label || '') + ' ' + _this.getMinSec(context.raw);
            }
          }
        }
      };
      options.scales = {
        y: {
          ticks: {
            callback: function (val, index) {
              return _this.getMinSec(val);
            }
          }
        }
      };
    } else if (type === 'Pack') {
      options.scales.yAxes[0].ticks.suggestedMax = max;
      options.scales.yAxes[0].ticks.suggestedMin = min;
      options.scales.yAxes[0].ticks.stepSize = 600;
      options.plugins = {
        tooltip: {
          callbacks: {
            label: function (context) {
              return (context.dataset.label || '') + " " + _this.getHourMinSec(context.raw);
            }
          }
        }
      };
      options.scales = {
        y: {
          ticks: {
            callback: function (val, index) {
              return _this.getHourMin(val);
            }
          },
          title: {
            display: true,
            text: 'Time of day'
          }
        }
      };
    } else if (type === 'Position') {
      options.scales.yAxes[0].ticks.max = max; // runners.length;
      options.scales.yAxes[0].ticks.min = 1;
      options.scales.yAxes[0].ticks.stepSize = 1;
      options.scales = {
        y: {
          title: {
            display: true,
            text: 'Position'
          }
        }
      };
    } else if (type === 'Performance') {
      options.scales.yAxes[0].ticks.suggestedMax = 200;
      options.scales.yAxes[0].ticks.suggestedMin = 0;
      options.scales.yAxes[0].ticks.stepSize = 5;
      options.scales = {
        y: {
          title: {
            display: true,
            text: 'Performance Index'
          }
        }
      };
    } else if (type === 'PerformanceHistogram') {
      options.scales.yAxes[0].ticks.max = max; // (runners || [])[0].splits.length / 2;
      options.scales.yAxes[0].ticks.min = 0;
      options.scales.yAxes[0].ticks.stepSize = 1;
      options.scales = {
        y: {
          title: {
            display: true,
            text: 'Occurences'
          }
        }
      };
    } else if (type === 'Mistakes') {
      options.plugins = {
        tooltip: {
          callbacks: {
            label: function (context) {
              return (context.dataset.label || '') + ' ' + _this.getMinSec(context.raw);
            }
          }
        }
      };
      options.scales = {
        y: {
          ticks: {
            callback: function (val, index) {
              return _this.getMinSec(val);
            }
          }
        }
      };
    }

    return options;
  },

  to2Digits: function (input) {
    return ('00' + input.toFixed(0).toString()).slice(-2);
  },

  getHourMin(input) {
    let value = Math.abs(input);
    const hourMod = value % 3600;
    const hourVal = ((value - hourMod) / 3600);
    const hour = hourVal.toFixed(0);
    value -= (hourVal * 3600);
    const minMod = value % 60;
    const min = ((value - minMod) / 60);

    return hour + ':' + this.to2Digits(min);
  },

  getHourMinSec(input) {
    let value = Math.abs(input);

    const hourMod = value % 3600;
    const hourVal = ((value - hourMod) / 3600);
    const hour = hourVal.toFixed(0);
    value -= (hourVal * 3600);
    const minMod = value % 60;
    const min = ((value - minMod) / 60);

    return hour + ':' + this.to2Digits(min) + ':' + this.to2Digits(minMod);
  },

  getMinSec(input) {
    let value = Math.abs(input);
    const mod = value % 60;
    const min = ((value - mod) / 60);
    return this.to2Digits(min) + ':' + this.to2Digits(mod);
  }
};