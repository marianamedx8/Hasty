import React from 'react';
import { Card } from 'react-bootstrap';
import { Bar, withResponsiveness } from 'britecharts-react';
import PropTypes from 'prop-types';

function ByMonth(props) {
  const { monthData, month } = props;
  const ResponsiveBarChart = withResponsiveness(Bar);
  const chartContainerStyle = {
    width: '100%',
    height: '300px',
  };

  const barChartData = [
    { name: 'Jan', value: 2 },
    { name: 'Feb', value: 2 },
    { name: month, value: monthData },
    { name: 'Apr', value: 0 },
    { name: 'Jun', value: 0 },
    { name: 'Jul', value: 0 },
    { name: 'Aug', value: 0 },
    { name: 'Sep', value: 0 },
    { name: 'Aug', value: 0 },
    { name: 'Dec', value: 0 },
  ];

  return (
    <Card>
      <Card.Body>
        <h4 className="header-title mb-4">Monthly Subscriptions</h4>
        <div className="bar-container" style={chartContainerStyle}>
          <ResponsiveBarChart
            isAnimated={false}
            data={barChartData}
            isHorizontal={false}
            height={300}
            betweenBarsPadding={0.5}
            colorSchema={['#39afd1']}
            margin={{ top: 10, left: 55, bottom: 20, right: 10 }}
          />
        </div>
      </Card.Body>
    </Card>
  );
}
ByMonth.propTypes = {
  monthData: PropTypes.number.isRequired,
  month: PropTypes.string.isRequired,
};

export default ByMonth;
