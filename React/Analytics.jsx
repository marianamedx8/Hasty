import React, { useEffect, useState } from 'react';
import { Card } from 'react-bootstrap';
import Chart from 'react-apexcharts';
import listingReservation from '../../../services/listingReservation';
import toastr from 'toastr';

function Analytics() {
    const [chartData, setChartData] = useState({
        data: [],
        dataName: [],
        popular: [],
    });
    useEffect(() => {
        listingReservation.getPopular().then(onGetPopularSuccess).catch(onGetPopularError);
    }, []);

    const onGetPopularSuccess = (response) => {
        const dataArr = response.items;
        setChartData((prevState) => {
            const newData = { ...prevState };
            newData.data = dataArr;
            newData.dataName = dataArr.map((data) => data.internalName);
            newData.popular = dataArr.map((data) => data.totalRepetitions);
            return newData;
        });
    };

    const onGetPopularError = () => {
        toastr['error']('No data to show');
    };

    const apexDonutOpts = {
        chart: {
            height: 320,
            type: 'pie',
        },
        labels: chartData.dataName,
        colors: ['#2E1A47', '#5C2563', '#FF9126', '#fa5c7c'],
        legend: {
            show: true,
            position: 'bottom',
            horizontalAlign: 'center',
            verticalAlign: 'middle',
            floating: false,
            fontSize: '14px',
            offsetX: 0,
            offsetY: -10,
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
    const apexDonutData = chartData.popular;
    return (
        <div className="mt-4">
            <Card className="file-height">
                <Card.Body>
                    <h4 className="header-title mb-3">Top Listings</h4>
                    <Chart
                        options={apexDonutOpts}
                        series={apexDonutData}
                        type="donut"
                        height={320}
                        className="apex-charts"
                    />
                </Card.Body>
            </Card>
        </div>
    );
}
export default Analytics;
