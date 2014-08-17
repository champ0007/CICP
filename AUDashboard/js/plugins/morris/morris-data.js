/// <reference path="morris.js" />
$(function () {

    

    Morris.Donut({
        element: 'morris-donut-chart1',
        data: [{
            label: "Running Projects",
            value: 12
        }, {
            label: "Lost Projects",
            value: 6
        }, {
            label: "Proposed Projects",
            value: 8
        }],
        resize: true
    });
   

    Morris.Bar({
        element: 'morris-bar-chart',
        data: [{
            y: 'Sitecore',
            a: 30            
        }, {
            y: 'CQ',
            a: 35            
        }, {
            y: 'Testing',
            a: 20           
        }],
        xkey: 'y',
        ykeys: ['a'],
        labels: ['Series A'],
        hideHover: 'auto',
        resize: true
    });

    

});
