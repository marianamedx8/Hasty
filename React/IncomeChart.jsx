import React from 'react';
import Chart from 'react-apexcharts';
import { Card } from 'react-bootstrap';
import PropTypes from 'prop-types';
import './admindash.css';

function IncomeChart(props) {
  const { names, income } = props;
  const apexDonutOpts = {
    chart: {
      height: 320,
      type: 'pie',
    },
    labels: names,
    colors: ['#5C2563', '#FF9126', '#39afd1'],
    legend: {
      show: true,
      position: 'bottom',
      horizontalAlign: 'center',
      verticalAlign: 'center',
      floating: true,
      fontSize: '14px',
      offsetX: 0,
      offsetY: 77,
    },
    responsive: [
      {
        breakpoint: 600,
        options: {
          chart: {
            height: 240,
          },
          legend: {
            show: false,
          },
        },
      },
    ],
  };

  const apexDonutData = income;

  return (
    <Card className="height-pie">
      <Card.Body>
        <h4 className="header-title mb-3">Income Per Plan</h4>
        <Chart options={apexDonutOpts} series={apexDonutData} type="pie" height={320} className="apex-charts" />
      </Card.Body>
    </Card>
  );
}
IncomeChart.propTypes = {
  names: PropTypes.string.isRequired,
  income: PropTypes.string.isRequired,
};

export default IncomeChart;
