var ForestAMetricsDataInterval = 360;
var ReadMemoryMetricsDataInterval = 60;
var ReadADSaveLatencyInterval = 60;
var TenantProvisionfailuresInterval = 60;
var TenantSubProvisionFailuresInterval = 60;
var TenantUserPublishFailuresInterval = 60;
var UserSubProvisionFailuresInterval = 60;
var UserProvisionFailuresInterval = 60;
var MNCUsersInterval = 60;
var ProvisionQueueInterval = 60;
var SubProvisionQueueInterval = 60;
var PublishingQueueInterval = 60;
window.onload = callAjax();
function hourlyData(hours) {
    var minutes = hours * 60;
    ForestAMetricsDataInterval = minutes;
    ReadMemoryMetricsDataInterval = minutes;
    ReadADSaveLatencyInterval = minutes;
    TenantProvisionfailuresInterval = minutes;
    TenantSubProvisionFailuresInterval = minutes;
    TenantUserPublishFailuresInterval = minutes;
    UserSubProvisionFailuresInterval = minutes;
    callAjax();
}
function dayData(days) {
    var minutes = days * 24 * 60;
    ForestAMetricsDataInterval = minutes;
    ReadMemoryMetricsDataInterval = minutes;
    ReadADSaveLatencyInterval = minutes;
    TenantProvisionfailuresInterval = minutes;
    TenantSubProvisionFailuresInterval = minutes;
    TenantUserPublishFailuresInterval = minutes;
    UserSubProvisionFailuresInterval = minutes;
    callAjax();
}
function callAjax() {
    hideAllDivShowLoadingDiv();
    $.when(
        $.post('/MSODS_Dashboard/ReadForestAMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainCTPM').style.display = 'block';
            document.getElementById('CTPM').style.display = 'block';
            document.getElementById('CTPM').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest1AMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor1A').style.display = 'block';
            document.getElementById('For1A').style.display = 'block';
            document.getElementById('For1A').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest2AMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor2A').style.display = 'block';
            document.getElementById('For2A').style.display = 'block';
            document.getElementById('For2A').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest3AMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor3A').style.display = 'block';
            document.getElementById('For3A').style.display = 'block';
            document.getElementById('For3A').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest4AMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor4A').style.display = 'block';
            document.getElementById('For4A').style.display = 'block';
            document.getElementById('For4A').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestBMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForB').style.display = 'block';
            document.getElementById('ForB').style.display = 'block';
            document.getElementById('ForB').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest1BMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor1B').style.display = 'block';
            document.getElementById('For1B').style.display = 'block';
            document.getElementById('For1B').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestEMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForE').style.display = 'block';
            document.getElementById('ForE').style.display = 'block';
            document.getElementById('ForE').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest1EMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor1E').style.display = 'block';
            document.getElementById('For1E').style.display = 'block';
            document.getElementById('For1E').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest2EMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor2E').style.display = 'block';
            document.getElementById('For2E').style.display = 'block';
            document.getElementById('For2E').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest3EMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor3E').style.display = 'block';
            document.getElementById('For3E').style.display = 'block';
            document.getElementById('For3E').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest4EMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor4E').style.display = 'block';
            document.getElementById('For4E').style.display = 'block';
            document.getElementById('For4E').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestFMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForF').style.display = 'block';
            document.getElementById('ForF').style.display = 'block';
            document.getElementById('ForF').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest0GMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor0G').style.display = 'block';
            document.getElementById('For0G').style.display = 'block';
            document.getElementById('For0G').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest1GMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor1G').style.display = 'block';
            document.getElementById('For1G').style.display = 'block';
            document.getElementById('For1G').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest2GMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor2G').style.display = 'block';
            document.getElementById('For2G').style.display = 'block';
            document.getElementById('For2G').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest0MMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor0M').style.display = 'block';
            document.getElementById('For0M').style.display = 'block';
            document.getElementById('For0M').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForest1MMetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainFor1M').style.display = 'block';
            document.getElementById('For1M').style.display = 'block';
            document.getElementById('For1M').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestAN1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForAN1').style.display = 'block';
            document.getElementById('ForAN1').style.display = 'block';
            document.getElementById('ForAN1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestAU1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForAU1').style.display = 'block';
            document.getElementById('ForAU1').style.display = 'block';
            document.getElementById('ForAU1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestCA1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForCA1').style.display = 'block';
            document.getElementById('ForCA1').style.display = 'block';
            document.getElementById('ForCA1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestDE1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForDE1').style.display = 'block';
            document.getElementById('ForDE1').style.display = 'block';
            document.getElementById('ForDE1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED1').style.display = 'block';
            document.getElementById('ForED1').style.display = 'block';
            document.getElementById('ForED1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED2MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED2').style.display = 'block';
            document.getElementById('ForED2').style.display = 'block';
            document.getElementById('ForED2').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED3MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED3').style.display = 'block';
            document.getElementById('ForED3').style.display = 'block';
            document.getElementById('ForED3').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED4MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED4').style.display = 'block';
            document.getElementById('ForED4').style.display = 'block';
            document.getElementById('ForED4').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED5MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED5').style.display = 'block';
            document.getElementById('ForED5').style.display = 'block';
            document.getElementById('ForED5').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestED6MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForED6').style.display = 'block';
            document.getElementById('ForED6').style.display = 'block';
            document.getElementById('ForED6').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestGB1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForGB1').style.display = 'block';
            document.getElementById('ForGB1').style.display = 'block';
            document.getElementById('ForGB1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestIN1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForIN1').style.display = 'block';
            document.getElementById('ForIN1').style.display = 'block';
            document.getElementById('ForIN1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestJP1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForJP1').style.display = 'block';
            document.getElementById('ForJP1').style.display = 'block';
            document.getElementById('ForJP1').innerHTML = htmlString;
        }),
        $.post('/MSODS_Dashboard/ReadForestKR1MetricsData', { interval: ForestAMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainForKR1').style.display = 'block';
            document.getElementById('ForKR1').style.display = 'block';
            document.getElementById('ForKR1').innerHTML = htmlString;
        })

    ).always(function () {
        document.getElementById('loadimgDivImage').style.display = 'none';
    });
}
function createMetricsUI(data) {
    var strHtml = "";
    if (data.length == 0) {
        strHtml += createAllForestButton();
    }
    else {
        for (var i = 0; i < data.length; i++) {
            var forestData = data[i];
            strHtml += createButton(forestData);
        }
    }
    return strHtml;
}
function createAllForestButton() {
    var buttonString = "";
    buttonString += "<button class='btn btn-primary dropdown-toggle' type ='button' data-toggle='dropdown' style='color:white;margin:4px;margin-left:0px;background-color: green'>Up to Date";
    buttonString += "</button>";
    return buttonString;
}
function createButton(forestData) {
    var buttonString = "";
    var tableString = "";
    var colorBtn = "<button class='btn btn-primary dropdown-toggle' type ='button' data-toggle='dropdown' style='color:white;margin:4px;margin-left:0px;background-color: " + forestData[0].Color + "'>" + forestData[0].ServiceInstanceShortName;
    var tableString = createTable(forestData);
    if (tableString.includes("<td>")) {
        buttonString += "<span class='dropdown'>";
        buttonString += colorBtn;
        buttonString += "</button>";
        buttonString += "<span class='dropdown-menu' style='padding: 0; margin: 0;position: fixed;top: 50%;left: 50%;transform: translate(-50%, -50%);border: 1000px solid rgba(0,0,0,0.5);'>";
        return buttonString + "" + tableString + "</span></span>";
    }
    else {
        buttonString += colorBtn;
        buttonString += "</button>";
        return buttonString;
    }
}
function createTable(forestData) {
    var table = "";
    table += "<table class='table table-striped table-condensed table-bordered' style='margin:0px;padding:0px;white-space: nowrap;'>";
    table += "<thead style='background-color:lightgrey'>";
    var startTr = "<tr>";
    var trtdStr = "<tr>";
    var endTr = "</tr></thead>";
    var ServiceDescriptorSuffix = "<th>Service Descriptor Suffix</th>";
    var ServiceInstance = "<th>Service Instance</th>";
    //var time = "<th>Time</th>";
    //var value = "<th>Value</th>";
    var ServiceType = "<th>Service Type</th>";
    var SyncStreamIdentifier = "<th>SyncStream Identifier</th>";
    var evalResult = "<th>Evaluation Result</th>";
    for (var i = 0; i < forestData.length; i++) {
        if (forestData.length > 0) {
            if (i == 0) {
                if (forestData[i].DatapointsList.length > 0) {
                    table += startTr + "" + ServiceInstance + "" + ServiceType + "" + SyncStreamIdentifier + "" + evalResult + "" + endTr;
                }
                //else {
                //    table += startTr + "" + ServiceDescriptorSuffix + "" + ServiceInstance + "" + endTr;;
                //}
            }
            if (forestData[i].DatapointsList.length > 0) {
                var timeStamp = (forestData[i].DatapointsList[forestData[i].DatapointsList.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].DatapointsList)
                table += trtdStr;
                //table += "<td>" + forestData[i].ServiceDescriptorSuffix + "</td>";
                table += "<td>" + "<a href=" + forestData[i].URL + " target='_blank'>" + forestData[i].ServiceInstance + "</a>" + "</td>";
                table += "<td>" + forestData[i].ServiceType + "</td>";
                table += "<td>" + forestData[i].SyncStreamIdentifier + "</td>";
                table += "<td>" + forestData[i].EvaluatedResult + "</td>";
                table += "</tr>";
            }
            //else {
            //    table += trtdStr;
            //    table += "<td>" + forestData[i].ServiceDescriptorSuffix + "</td>";
            //    table += "<td>" + forestData[i].ServiceInstance + "</td>";
            //    table += "</tr>";
            //}
        }
        //else if (forestData[i].divId.includes("ADS")) {
        //    if (i == 0) {
        //        if (forestData[i].DatapointsList.length > 0) {
        //            table += startTr + "" + ServiceInstance + "" + time + "" + value + "" + endTr;;
        //        }
        //        else {
        //            table += startTr + "" + ServiceInstance + "" + endTr;;
        //        }
        //    }
        //    if (forestData[i].DatapointsList.length > 0) {
        //        var timeStamp = (forestData[i].Datapoints[forestData[i].DatapointsList.length - 1].TimestampUtc);
        //        value = getMaxValue(forestData[i].Datapoints)
        //        var date = getDateTime(timeStamp);
        //        table += trtdStr;
        //        table += "<td>" + forestData[i].ServiceInstance + "</td>";
        //        table += "<td>" + date + "</td>";
        //        table += "<td>" + value + "</td>";
        //        table += "</tr>";
        //    }
        //    else {
        //        table += trtdStr;
        //        table += "<td>" + forestData[i].ServiceInstance + "</td>";
        //        table += "</tr>";
        //    }
        //}
        //else if (forestData[i].divId.includes("Mem")) {
        //    if (i == 0) {
        //        table += startTr + "" + forest + "" + ServiceDescriptorSuffix + "" + ServiceInstance + "" + percent + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Forest + "</td>";
        //    table += "<td>" + forestData[i].ServiceDescriptorSuffix + "</td>";
        //    table += "<td>" + forestData[i].ServiceInstance + "</td>";
        //    table += "<td>" + forestData[i].PercentageFree + "</td>";
        //    table += "</tr>";
        //}
        //else if (forestData[i].divId.includes("TPF")) {
        //    if (i == 0) {
        //        if (forestData[i].DatapointsList.length > 0) {
        //            table += startTr + "" + time + "" + value + "" + endTr;;
        //        }
        //    }
        //    if (forestData[i].DatapointsList.length > 0) {
        //        var timeStamp = (forestData[i].Datapoints[forestData[i].DatapointsList.length - 1].TimestampUtc);
        //        var date = getDateTime(timeStamp);
        //        value = getMaxValue(forestData[i].Datapoints)
        //        table += trtdStr;
        //        table += "<td>" + date + "</td>";
        //        table += "<td>" + value + "</td>";
        //        table += "</tr>";
        //    }
        //}
        //else if (forestData[i].divId.includes("TSPF")) {
        //    if (i == 0) {
        //        if (forestData[i].DatapointsList.length > 0) {
        //            table += startTr + "" + time + "" + value + "" + endTr;;
        //        }
        //    }
        //    if (forestData[i].DatapointsList.length > 0) {
        //        var timeStamp = (forestData[i].Datapoints[forestData[i].DatapointsList.length - 1].TimestampUtc);
        //        var date = getDateTime(timeStamp);
        //        value = getMaxValue(forestData[i].Datapoints);
        //        table += trtdStr;
        //        table += "<td>" + date + "</td>";
        //        table += "<td>" + value + "</td>";
        //        table += "</tr>";
        //    }
        //}
        //else if (forestData[i].divId.includes("TUPF")) {
        //    if (i == 0) {
        //        if (forestData[i].DatapointsList.length > 0) {
        //            table += startTr + "" + time + "" + value + "" + endTr;;
        //        }
        //    }
        //    if (forestData[i].DatapointsList.length > 0) {
        //        var timeStamp = (forestData[i].Datapoints[forestData[i].DatapointsList.length - 1].TimestampUtc);
        //        var date = getDateTime(timeStamp);
        //        value = getMaxValue(forestData[i].Datapoints)
        //        table += trtdStr;
        //        table += "<td>" + date + "</td>";
        //        table += "<td>" + value + "</td>";
        //        table += "</tr>";
        //    }
        //}
        //else if (forestData[i].divId.includes("USPF")) {
        //    if (i == 0) {
        //        if (forestData[i].DatapointsList.length > 0) {
        //            table += startTr + "" + time + "" + value + "" + endTr;;
        //        }
        //    }
        //    if (forestData[i].DatapointsList.length > 0) {
        //        var timeStamp = (forestData[i].Datapoints[forestData[i].DatapointsList.length - 1].TimestampUtc);
        //        var date = getDateTime(timeStamp);
        //        value = getMaxValue(forestData[i].Datapoints)
        //        table += trtdStr;
        //        table += "<td>" + date + "</td>";
        //        table += "<td>" + value + "</td>";
        //        table += "</tr>";
        //    }
        //}
        //else if (forestData[i].divId.includes("MnsUsr")) {
        //    if (i == 0) {
        //        table += startTr + "" + role + "" + evalResult + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Role + "</td>";
        //    table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
        //    table += "</tr>";
        //}
        //else if (forestData[i].divId.includes("divPQ")) {
        //    if (i == 0) {
        //        table += startTr + "" + role + "" + evalResult + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Role + "</td>";
        //    table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
        //    table += "</tr>";
        //}
        //else if (forestData[i].divId.includes("SPQ")) {
        //    if (i == 0) {
        //        table += startTr + "" + role + "" + evalResult + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Role + "</td>";
        //    table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
        //    table += "</tr>";
        //}
        //else if (forestData[i].divId.includes("PubQ")) {
        //    if (i == 0) {
        //        table += startTr + "" + role + "" + evalResult + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Role + "</td>";
        //    table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
        //    table += "</tr>";
        //}
        //else if (forestData[i].divId.includes("divUPF")) {
        //    if (i == 0) {
        //        table += startTr + "" + role + "" + evalResult + "" + endTr;;
        //    }
        //    table += trtdStr;
        //    table += "<td>" + forestData[i].Role + "</td>";
        //    table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
        //    table += "</tr>";
        //}
    }
    table += "</table>";
    return table;
}
function getDateTime(timeStamp) {
    timeStamp = timeStamp.substring(6, timeStamp.length - 2);
    timeStamp = parseInt(timeStamp);
    var date = new Date(timeStamp);
    return date.toLocaleDateString() + " " + date.toLocaleTimeString();
}
function getMaxValue(forestData) {
    var value = forestData.reduce((max, p) => p.Value < max ? p.Value : max, forestData[0].Value);
    return getTwoDecimalPointValue(value);
}
function getTwoDecimalPointValue(value) {
    return value.toFixed(2);
}
function hideAllDivShowLoadingDiv() {
    document.getElementById('mainCTPM').style.display = 'none';
    //document.getElementById('mainMU').style.display = 'none';
    //document.getElementById('CTPM').style.display = 'none';
    //document.getElementById('MU').style.display = 'none';
    //document.getElementById('mainAdLatency').style.display = 'none';
    //document.getElementById('mainTPF').style.display = 'none';
    //document.getElementById('mainTSPF').style.display = 'none';
    //document.getElementById('mainTPublish').style.display = 'none';
    //document.getElementById('mainUSPF').style.display = 'none';
    //document.getElementById('mainUPF').style.display = 'none';
    //document.getElementById('UPF').style.display = 'none';
    //document.getElementById('mainMNCUSF').style.display = 'none';
    //document.getElementById('MNCUSF').style.display = 'none';
    //document.getElementById('mainPQ').style.display = 'none';
    //document.getElementById('PQ').style.display = 'none';
    //document.getElementById('mainSPQ').style.display = 'none';
    //document.getElementById('SPQ').style.display = 'none';
    //document.getElementById('mainPubQ').style.display = 'none';
    document.getElementById('loadimgDivImage').style.display = 'block';
}

