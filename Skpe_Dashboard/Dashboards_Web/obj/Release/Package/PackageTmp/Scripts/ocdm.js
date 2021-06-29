var ReadCPUMetricsDataInterval = 60;
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
    ReadCPUMetricsDataInterval = minutes;
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
    ReadCPUMetricsDataInterval = minutes;
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
        $.post('/OCDM_Health_Dashboard/ReadCPUMetricsData', { interval: ReadCPUMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainCTPM').style.display = 'block';
            document.getElementById('CTPM').style.display = 'block';
            document.getElementById('CTPM').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/ReadMemoryMetricsData', { interval: ReadMemoryMetricsDataInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainMU').style.display = 'block';
            document.getElementById('MU').style.display = 'block';
            document.getElementById('MU').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/ReadADSaveLatency', { interval: ReadADSaveLatencyInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainAdLatency').style.display = 'block';
            document.getElementById('AdLatency').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/TenantProvisionfailures', { interval: TenantProvisionfailuresInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainTPF').style.display = 'block';
            document.getElementById('TPF').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/TenantSubProvisionFailures', { interval: TenantSubProvisionFailuresInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainTSPF').style.display = 'block';
            document.getElementById('TSPF').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/TenantUserPublishFailures', { interval: TenantUserPublishFailuresInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainTPublish').style.display = 'block';
            document.getElementById('TPublish').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/UserSubProvisionFailures', { interval: UserSubProvisionFailuresInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainUSPF').style.display = 'block';
            document.getElementById('USPF').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/UserProvisionFailures', { interval: UserProvisionFailuresInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainUPF').style.display = 'block';
            document.getElementById('UPF').style.display = 'block';
            document.getElementById('UPF').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/ReadNumberofMNCUsersFailed', { interval: MNCUsersInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainMNCUSF').style.display = 'block';
            document.getElementById('MNCUSF').style.display = 'block';
            document.getElementById('MNCUSF').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/ProvisioningQueue', { interval: ProvisionQueueInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainPQ').style.display = 'block';
            document.getElementById('PQ').style.display = 'block';
            document.getElementById('PQ').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/SubProvisioningQueue', { interval: SubProvisionQueueInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainSPQ').style.display = 'block';
            document.getElementById('SPQ').style.display = 'block';
            document.getElementById('SPQ').innerHTML = htmlString;
        }),
        $.post('/OCDM_Health_Dashboard/PublishingQueue', { interval: PublishingQueueInterval }, function (data) {
            var htmlString = createMetricsUI(data);
            document.getElementById('mainPubQ').style.display = 'block';
            document.getElementById('PubQ').innerHTML = htmlString;
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
    buttonString += "<button class='btn btn-primary dropdown-toggle' type ='button' data-toggle='dropdown' style='color:white;margin:4px;margin-left:0px;background-color: green'>All Forest";
    buttonString += "</button>";
    return buttonString;
}
function createButton(forestData) {
    var buttonString = "";
    var tableString = "";
    var colorBtn = "<button class='btn btn-primary dropdown-toggle' type ='button' data-toggle='dropdown' style='color:white;margin:4px;margin-left:0px;background-color: " + forestData[0].DivColor + "'>" + forestData[0].Forest;
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
    var machine = "<th>Machine</th>";
    var forest = "<th>Forest</th>";
    var time = "<th>Time</th>";
    var value = "<th>Value</th>";
    var poolFqdn = "<th>PoolFQDN</th>";
    var percent = "<th>Percentage Free</th>";
    var role = "<th>Role</th>";
    var evalResult = "<th>Evaluation Result</th>";
    for (var i = 0; i < forestData.length; i++) {
        if (forestData[i].divId.includes("CPU")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + machine + "" + poolFqdn + "" + time + "" + value + "" + endTr;
                }
                else {
                    table += startTr + "" + machine + "" + poolFqdn + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].Datapoints)
                table += trtdStr;
                table += "<td>" + forestData[i].Machine + "</td>";
                table += "<td>" + forestData[i].PoolFQDN + "</td>";
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
            else {
                table += trtdStr;
                table += "<td>" + forestData[i].Machine + "</td>";
                table += "<td>" + forestData[i].PoolFQDN + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("ADS")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + poolFqdn + "" + time + "" + value + "" + endTr;;
                }
                else {
                    table += startTr + "" + poolFqdn + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                value = getMaxValue(forestData[i].Datapoints)
                var date = getDateTime(timeStamp);
                table += trtdStr;
                table += "<td>" + forestData[i].PoolFQDN + "</td>";
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
            else {
                table += trtdStr;
                table += "<td>" + forestData[i].PoolFQDN + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("Mem")) {
            if (i == 0) {
                table += startTr + "" + forest + "" + machine + "" + poolFqdn + "" + percent + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Forest + "</td>";
            table += "<td>" + forestData[i].Machine + "</td>";
            table += "<td>" + forestData[i].PoolFQDN + "</td>";
            table += "<td>" + forestData[i].PercentageFree + "</td>";
            table += "</tr>";
        }
        else if (forestData[i].divId.includes("TPF")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + time + "" + value + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].Datapoints)
                table += trtdStr;
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("TSPF")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + time + "" + value + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].Datapoints);
                table += trtdStr;
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("TUPF")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + time + "" + value + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].Datapoints)
                table += trtdStr;
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("USPF")) {
            if (i == 0) {
                if (forestData[i].Datapoints.length > 0) {
                    table += startTr + "" + time + "" + value + "" + endTr;;
                }
            }
            if (forestData[i].Datapoints.length > 0) {
                var timeStamp = (forestData[i].Datapoints[forestData[i].Datapoints.length - 1].TimestampUtc);
                var date = getDateTime(timeStamp);
                value = getMaxValue(forestData[i].Datapoints)
                table += trtdStr;
                table += "<td>" + date + "</td>";
                table += "<td>" + value + "</td>";
                table += "</tr>";
            }
        }
        else if (forestData[i].divId.includes("MnsUsr")) {
            if (i == 0) {
                table += startTr + "" + role + "" + evalResult + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Role + "</td>";
            table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
            table += "</tr>";
        }
        else if (forestData[i].divId.includes("divPQ")) {
            if (i == 0) {
                table += startTr + "" + role + "" + evalResult + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Role + "</td>";
            table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
            table += "</tr>";
        }
        else if (forestData[i].divId.includes("SPQ")) {
            if (i == 0) {
                table += startTr + "" + role + "" + evalResult + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Role + "</td>";
            table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
            table += "</tr>";
        }
        else if (forestData[i].divId.includes("PubQ")) {
            if (i == 0) {
                table += startTr + "" + role + "" + evalResult + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Role + "</td>";
            table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
            table += "</tr>";
        }
        else if (forestData[i].divId.includes("divUPF")) {
            if (i == 0) {
                table += startTr + "" + role + "" + evalResult + "" + endTr;;
            }
            table += trtdStr;
            table += "<td>" + forestData[i].Role + "</td>";
            table += "<td>" + getTwoDecimalPointValue(forestData[i].Result) + "</td>";
            table += "</tr>";
        }
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
    document.getElementById('mainMU').style.display = 'none';
    document.getElementById('CTPM').style.display = 'none';
    document.getElementById('MU').style.display = 'none';
    document.getElementById('mainAdLatency').style.display = 'none';
    document.getElementById('mainTPF').style.display = 'none';
    document.getElementById('mainTSPF').style.display = 'none';
    document.getElementById('mainTPublish').style.display = 'none';
    document.getElementById('mainUSPF').style.display = 'none';
    document.getElementById('mainUPF').style.display = 'none';
    document.getElementById('UPF').style.display = 'none';
    document.getElementById('mainMNCUSF').style.display = 'none';
    document.getElementById('MNCUSF').style.display = 'none';
    document.getElementById('mainPQ').style.display = 'none';
    document.getElementById('PQ').style.display = 'none';
    document.getElementById('mainSPQ').style.display = 'none';
    document.getElementById('SPQ').style.display = 'none';
    document.getElementById('mainPubQ').style.display = 'none';
    document.getElementById('loadimgDivImage').style.display = 'block';
}

