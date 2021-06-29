var trainingTracksArray = ["Generic", "Deployment", "Patching", "Imaging", "Corp", "Decommission", "Gallatin", "CVM"];
var accessTrackArray = ["All Tracks", "SOC - Infra", "CVM BGC"];
var genericTrackArray = ["All Tracks"];
var operationTrackArray = ["All Tracks", "Decommission", "Imaging", "Patching & Deployment", "Usermoves", "Vulnerability", "SOC - Infra", "SOC - Customer Bug", "CVM"];
var dashboardrackArray = ["All Tracks"];

var categoryDropDownId = "";
var categoryDropDownValue = "";

var allFreqDropDownId = "";
var allFreqDropDownValue = "";
var trackDropDownId = "";
var trackDropDownValue = "";

var processedJsonData = "";
var sortSelectedValue = "";
var divider = "";


function getTracks() {
    categoryDropDownId = document.getElementById("dropdownCategory");
    categoryDropDownValue = categoryDropDownId.value;
    if (categoryDropDownValue == "Access") {
        createSelectOptionDropdown(accessTrackArray);
    } else if (categoryDropDownValue == "Generic") {
        createSelectOptionDropdown(genericTrackArray);
    }
    else if (categoryDropDownValue == "Dashboard") {
        createSelectOptionDropdown(dashboardrackArray);
    }
    else if (categoryDropDownValue == "Training") {
        createSelectOptionDropdown(trainingTracksArray);

    } else if (categoryDropDownValue == "Operations") {
        createSelectOptionDropdown(operationTrackArray);
    }
    else if (categoryDropDownValue == "select") {
        createSelectOptionDropdown(null);
    }
    else {
        createSelectOptionDropdown(null);
    }
}

function createSelectOptionDropdown(trackDropDownArray) {
    document.getElementById('dropdownTrack').innerHTML = "";
    if (trackDropDownArray == null) {
        var newSelect = document.createElement('option');
        selectHTML = "<option selected='selected' value='select'>Select Track</option>";
        newSelect.innerHTML = selectHTML;
        document.getElementById('dropdownTrack').add(newSelect);
        $('#dropdownTrack').selectpicker('refresh');
        trackDropDownId.disabled = true;
    } else {
        trackDropDownArray.sort();
        for (var i = 0; i < trackDropDownArray.length; i++) {
            var newSelect = document.createElement('option');
            if (i == 0) {
                selectHTML = "<option selected='selected' value='" + trackDropDownArray[i] + "'>" + trackDropDownArray[i] + "</option>";
            } else {
                selectHTML = "<option value='" + trackDropDownArray[i] + "'>" + trackDropDownArray[i] + "</option>";
            }
            newSelect.innerHTML = selectHTML;
            trackDropDownId.add(newSelect);
            $('#dropdownTrack').selectpicker('refresh');
            trackDropDownId.disabled = false;
        }
    }
    $('#dropdownTrack').selectpicker('refresh');
    getJsonData();
}



function processJsonData() {
    trackDropDownId = document.getElementById("dropdownTrack");
    trackDropDownValue = trackDropDownId.options[trackDropDownId.selectedIndex].text;
    if (categoryDropDownValue == "Access") {
        if (trackDropDownValue == accessTrackArray[0]) {
            processedJsonData = accessJsonData.allTracks;
        }
        else if (trackDropDownValue == "SOC - Infra") {
            processedJsonData = accessJsonData.infra;
        }
        else if (trackDropDownValue == "CVM BGC") {
            processedJsonData = accessJsonData.bgc;
        }
    }
    else if (categoryDropDownValue == "Generic") {
        if (trackDropDownValue == "All Tracks") {
            processedJsonData = genericJsonData.allTracks;
        }
    }
    else if (categoryDropDownValue == "Dashboard") {
        if (trackDropDownValue == "All Tracks") {
            processedJsonData = dashboardJsonData.allTracks;
        }
    }
    else if (categoryDropDownValue == "Training") {
        if (trackDropDownValue == "Generic") {
            processedJsonData = trainingJsonData.generic;
        }
        else if (trackDropDownValue == "Deployment") {
            processedJsonData = trainingJsonData.deployment;
        }
        else if (trackDropDownValue == "Patching") {
            processedJsonData = trainingJsonData.patching;
        }
        else if (trackDropDownValue == "Imaging") {
            processedJsonData = trainingJsonData.imaging;
        }
        else if (trackDropDownValue == "Corp") {
            processedJsonData = trainingJsonData.corp;
        }
        else if (trackDropDownValue == "Decommission") {
            processedJsonData = trainingJsonData.decommission;
        }
        else if (trackDropDownValue == "CVM") {
            divider = "yes";
        }
        else {
            processedJsonData = trainingJsonData.gallatin;
        }

    }
    else if (categoryDropDownValue == "Operations") {
        if (trackDropDownValue == "All Tracks") {
            processedJsonData = operationJsonData.allTracks;
        }

        else if (trackDropDownValue == "Decommission") {
            processedJsonData = operationJsonData.decommission;
        }
        else if (trackDropDownValue == "Imaging") {
            divider = "yes";
        }
        else if (trackDropDownValue == "Patching & Deployment") {
            divider = "yes";
        }
        else if (trackDropDownValue == "Usermoves") {
            divider = "yes";
        }
        else if (trackDropDownValue == "SOC - Infra") {
            divider = "yes";
        }
        else if (trackDropDownValue == "SOC - Customer Bug") {
            divider = "yes";
        }
        else if (trackDropDownValue == "CVM") {
            processedJsonData = operationJsonData.cvm;
        }
        //else if (trackDropDownValue == "CVM All Tracks") {
        //    processedJsonData = operationJsonData.cvmAllTracks;
        //}
        else {
            processedJsonData = operationJsonData.vulnerability;
        }
    }
    else {
        //alert("Please select any value");
    }
}

function getJsonData() {
    var stringHtml = "";
    var startToolRow = '<div class="row commontoolsopheading">Tool</div>';
    var startSOPRow = '<div class="row commontoolsopheading sopheading">Wiki</div>';
    var divClose = '</div>';
    document.getElementById('linksContainer').innerHTML = "";
    processJsonData();

    if (categoryDropDownValue == "select" || categoryDropDownValue == "") {
        trackDropDownId.disabled = true;
        setInitialData();
        sortForToolAndSOP();
        for (var i = 0; i < processedJsonData.length; i++) {
            stringHtml += makeString(i);
        }
    }
    else if (divider == 'yes') {
        if (trackDropDownValue == 'Patching & Deployment') {
            processedJsonData = operationJsonData.patchingAndDeployment;
            stringHtml += startToolRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.patchingAndDeployment.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;

            processedJsonData = operationJsonData.patchingAndDeploymentSop;
            sortForToolAndSOP();

            stringHtml += startSOPRow;
            for (var i = 0; i < operationJsonData.patchingAndDeploymentSop.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;
            processedJsonData = operationJsonData.patchingAndDeployment.concat(operationJsonData.patchingAndDeploymentSop);
        }
        else if (trackDropDownValue == 'Imaging') {
            processedJsonData = operationJsonData.imaging;
            stringHtml += startToolRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.imaging.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;
            processedJsonData = operationJsonData.imagingSop;
            stringHtml += startSOPRow
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.imagingSop.length; i++) {
                stringHtml += makeString(i);
            }
            processedJsonData = operationJsonData.imaging.concat(operationJsonData.imagingSop);
            stringHtml += divClose;
        }
        else if (trackDropDownValue == "SOC - Infra") {
            processedJsonData = operationJsonData.infra;
            stringHtml += startToolRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.infra.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;
            processedJsonData = operationJsonData.infraSOP;
            stringHtml += startSOPRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.infraSOP.length; i++) {
                stringHtml += makeString(i);
            }
            processedJsonData = operationJsonData.infra.concat(operationJsonData.infraSOP);
            stringHtml += divClose
        }
        else if (trackDropDownValue == "SOC - Customer Bug") {
            processedJsonData = operationJsonData.customerBugSop;
            stringHtml += startSOPRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.customerBugSop.length; i++) {
                stringHtml += makeString(i);
            }
            processedJsonData = operationJsonData.customerBugSop;
            stringHtml += divClose
        }
        else if (trackDropDownValue == "CVM") {
            processedJsonData = trainingJsonData.cvm;
            stringHtml += startToolRow;
            sortForToolAndSOP();
            for (var i = 0; i < trainingJsonData.cvm.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;
            processedJsonData = trainingJsonData.cvmSop;
            stringHtml += startSOPRow
            sortForToolAndSOP();
            for (var i = 0; i < trainingJsonData.cvmSop.length; i++) {
                stringHtml += makeString(i);
            }
            processedJsonData = trainingJsonData.cvm.concat(trainingJsonData.cvmSop);
            stringHtml += divClose;
        }
        else {
            processedJsonData = operationJsonData.usermoves;
            stringHtml += startToolRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.usermoves.length; i++) {
                stringHtml += makeString(i);
            }
            stringHtml += divClose;
            processedJsonData = operationJsonData.usermovesSop;
            stringHtml += startSOPRow;
            sortForToolAndSOP();
            for (var i = 0; i < operationJsonData.usermovesSop.length; i++) {
                stringHtml += makeString(i);
            }
            processedJsonData = operationJsonData.usermoves.concat(operationJsonData.usermovesSop);
            stringHtml += divClose
        }
    }
    else {
        sortForToolAndSOP();
        for (var i = 0; i < processedJsonData.length; i++) {
            stringHtml += makeString(i);
        }
    }
    divider = "";
    document.getElementById('linksContainer').innerHTML = stringHtml;
}



// string maker for html
function makeString(index) {
    var result = "";
    if (index == 0) {
        result += '<div class="row rowImageContainer">';
    }
    var colCounter = index + 1;
    result += "<div class='col-lg-2 col-sm-2 col-xs-2 imageConatinerCols'>" +
        '<a  href="' + processedJsonData[index].link + '"target="_blank"> <img src="Content/img/' + processedJsonData[index].imgPath + '" class="img-thumbnail" alt="' + processedJsonData[index].friendlyName + '"></a>' +
        '<div>' +
        '<ul class="list-group list-group-flush titleDesc">' +
        '<a  href="' + processedJsonData[index].link + '"target="_blank"><li class="list-group-item titleText">' +
        processedJsonData[index].friendlyName + '</li></a>' +
        '<li class="list-group-item descText">' + processedJsonData[index].description + '</li>' +
        '</ul>' +
        '</div>' +
        '</div>';
    if (colCounter % 6 == 0) {
        result += '</div>';
        result += '<div class="row rowImageContainer">';
    }
    return result;

}
function sortLinks() {
    document.getElementById('searchField').value = "";
    var sortdropdowElement = document.getElementById("sortdropdown");
    sortSelectedValue = sortdropdowElement.options[sortdropdowElement.selectedIndex].text;
    getJsonData();
}
function sortForToolAndSOP() {
    if (sortSelectedValue == 'Descending') {
        processedJsonData.sort(GetSortOrder("friendlyName")).reverse(GetSortOrder("friendlyName"));
    }
    else if (sortSelectedValue == 'Ascending') {
        processedJsonData.sort(GetSortOrder("friendlyName"));
    }
    else {
        return;
    }
}
//Comparer Function    
function GetSortOrder(prop) {
    return function (a, b) {
        if (a[prop] > b[prop]) {
            return 1;
        } else if (a[prop] < b[prop]) {
            return -1;
        }
        return 0;
    }
}
function searchLinks() {
    var textFromSearchBox = document.getElementById('searchField').value.toLowerCase();
    if (textFromSearchBox == "") {
        getJsonData();
        return;
    } else {
        var stringHtml = "";
        for (var i = 0; i < processedJsonData.length; i++) {
            if (processedJsonData[i].friendlyName.toLowerCase().trim().includes(textFromSearchBox)) {
                stringHtml += makeString(i);
            }
        }
        document.getElementById('linksContainer').innerHTML = stringHtml;
    }
}
function setInitialData() {
    allFreqDropDownValue = allFreqDropDownId.value;
    if (allFreqDropDownValue == "SD Frequently Used") {
        processedJsonData = frequentlyUsedJsonData.sdfreqUsed;
    }
    else if (allFreqDropDownValue == "SOC Frequently Used") {
        processedJsonData = frequentlyUsedJsonData.SocfreqUsed;
    }
    else if (allFreqDropDownValue == "CVM Frequently Used") {
        processedJsonData = frequentlyUsedJsonData.cvmFreqUsed;
    }
    else {
        processedJsonData =
            trainingJsonData.generic.concat(trainingJsonData.corp)
                .concat(trainingJsonData.decommission)
                .concat(trainingJsonData.deployment)
                .concat(trainingJsonData.imaging)
                .concat(trainingJsonData.patching)
                .concat(trainingJsonData.gallatin)
                .concat(trainingJsonData.cvm)
                .concat(trainingJsonData.cvmSop)
                .concat(operationJsonData.allTracks)
                .concat(operationJsonData.patchingAndDeployment).concat(operationJsonData.patchingAndDeploymentSop)
                .concat(operationJsonData.imaging).concat(operationJsonData.imagingSop)
                .concat(operationJsonData.decommission)
                .concat(operationJsonData.usermoves).concat(operationJsonData.usermovesSop)
                .concat(operationJsonData.vulnerability)
                .concat(operationJsonData.infra)
                .concat(operationJsonData.infraSOP)
                .concat(operationJsonData.customerBugSop)
                .concat(operationJsonData.cvm)
                .concat(operationJsonData.cvmAllTracks)
                .concat(accessJsonData.allTracks)
                .concat(accessJsonData.infra)
                .concat(accessJsonData.bgc)
                .concat(genericJsonData.allTracks)
                .concat(dashboardJsonData.allTracks);
    }
}
// onload function
function onLoadPage() {
    categoryDropDownId = document.getElementById("dropdownCategory");
    allFreqDropDownId = document.getElementById("dropdownAllFreq");
    getJsonData();
}
window.onload = onLoadPage;
// Json Data for links
var trainingJsonData = {
    "generic": [
        {
            "id": "1",
            "friendlyName": "Lync KT Document",
            "category": "Training",
            "track": "Generic",
            "description": "This document will help new joiners to understand the overview of skype for business",
            "link": "https://microsoft.sharepoint.com/:w:/r/teams/ucops/_layouts/15/Doc.aspx?sourcedoc=%7BABF6D595-81D3-42F1-A363-745F03DCFA9A%7D&file=Lync%20KT%20Doc.docx&action=default&mobileredirect=true",
            "imgPath": "Training_1.jpg"
        }
    ],
    "corp": [
        {
            "id": "16",
            "friendlyName": "Training Corp 1",
            "category": "Training",
            "track": "Corp",
            "description": "Corp Training Video 1",
            "link": "https://web.microsoftstream.com/video/7bcea3ff-0400-a9f4-6cc8-f1ea9e509882",
            "imgPath": "Training_16.jpg"
        }
    ],
    "decommission": [
        {
            "id": "14",
            "friendlyName": "Decommission Training 1",
            "category": "Training",
            "track": "Decommission",
            "description": "Decomission Training Video 1",
            "link": "https://web.microsoftstream.com/video/13fba3ff-0400-96d0-1021-f1ea9bfd32f3",
            "imgPath": "Training_14.jpg"
        },
        {
            "id": "15",
            "friendlyName": "Decommission Training 2",
            "category": "Training",
            "track": "Decommission",
            "description": "Decomission Training Video 2",
            "link": "https://web.microsoftstream.com/video/7bcea3ff-0400-a9f4-92c6-f1ea9d8f57f0",
            "imgPath": "Training_15.jpg"
        }
    ],
    "deployment": [
        {
            "id": "2",
            "friendlyName": "Deployment Training 1",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 1",
            "link": "https://web.microsoftstream.com/video/ea45a1ff-0400-96d1-4403-f1ea8e9cdbac",
            "imgPath": "Training_2.jpg"
        },
        {
            "id": "3",
            "friendlyName": "Deployment Training 2",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 2",
            "link": "https://web.microsoftstream.com/video/8b53a1ff-0400-96d1-85bd-f1ea8f673a8c",
            "imgPath": "Training_3.jpg"
        },
        {
            "id": "4",
            "friendlyName": "Deployment Training 3",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 3",
            "link": "https://web.microsoftstream.com/video/9538a4ff-0400-96d1-9a57-f1ea9032a9d8",
            "imgPath": "Training_4.jpg"
        },
        {
            "id": "5",
            "friendlyName": "Deployment Training 4",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 4",
            "link": "https://web.microsoftstream.com/video/621ba4ff-0400-96d0-77fb-f1ea90f8d0aa",
            "imgPath": "Training_5.jpg"
        },
        {
            "id": "6",
            "friendlyName": "Deployment Training 5",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 5",
            "link": "https://web.microsoftstream.com/video/4fcca3ff-0400-96d0-0e11-f1ea928b899f",
            "imgPath": "Training_6.jpg"
        },
        {
            "id": "7",
            "friendlyName": "Deployment Training 6",
            "category": "Training",
            "track": "Deployment",
            "description": "Deployment Training Video 6",
            "link": "https://web.microsoftstream.com/video/22f5a3ff-0400-96d0-677a-f1ea93560c59",
            "imgPath": "Training_7.jpg"
        }
    ],
    "imaging": [
        {
            "id": "11",
            "friendlyName": "Imaging Training 1",
            "category": "Training",
            "track": "Imaging",
            "description": "Imaging Training Video 1",
            "link": "https://web.microsoftstream.com/video/0f65a1ff-0400-96d0-0b6c-f1ea980a1bfd",
            "imgPath": "Training_11.jpg"
        },
        {
            "id": "12",
            "friendlyName": "Imaging Training 2",
            "category": "Training",
            "track": "Imaging",
            "description": "Imaging Training Video 2",
            "link": "https://web.microsoftstream.com/video/6c38a1ff-0400-96d0-7ecc-f1ea980361d2",
            "imgPath": "Training_12.jpg"
        },
        {
            "id": "13",
            "friendlyName": "Imaging Training 3",
            "category": "Training",
            "track": "Imaging",
            "description": "Imaging Training Video 3",
            "link": "https://web.microsoftstream.com/video/12fba3ff-0400-96d0-2dd4-f1ea9a665019",
            "imgPath": "Training_13.jpg"
        }
    ],
    "patching": [
        {
            "id": "8",
            "friendlyName": "Patching Training 1",
            "category": "Training",
            "track": "Patching",
            "description": "Patching Training Video 1",
            "link": "https://web.microsoftstream.com/video/b94ba1ff-0400-96d0-b318-f1ea941e06af",
            "imgPath": "Training_8.jpg"
        },
        {
            "id": "9",
            "friendlyName": "Patching Training 2",
            "category": "Training",
            "track": "Patching",
            "description": "Patching Training Video 2",
            "link": "https://web.microsoftstream.com/video/e707a4ff-0400-96d0-6498-f1ea94e56239",
            "imgPath": "Training_9.jpg"
        },
        {
            "id": "10",
            "friendlyName": "Patching Training 3",
            "category": "Training",
            "track": "Patching",
            "description": "Patching Training Video 3",
            "link": "https://web.microsoftstream.com/video/6ecea3ff-0400-96d0-e2a9-f1ea97431bb4",
            "imgPath": "Training_10.jpg"
        }
    ],
    "gallatin": [
        {
            "id": "17",
            "friendlyName": "Training Gallatin 1",
            "category": "Training",
            "track": "Gallatin",
            "description": "Gallatin Training Video 1",
            "link": "https://web.microsoftstream.com/video/70cea3ff-0400-a936-44f7-f1ea9fe7a57d",
            "imgPath": "Training_17.jpg"
        }
    ],
    "cvm": [
        {
            "id": "223",
            "friendlyName": "SAW New User Overview Video",
            "category": "Training",
            "track": "CVM",
            "description": "SAW New User Overview Video",
            "link": "https://microsoft.sharepoint.com/sites/itweb/secure-admin-services/Documents/Brown%20Bags/SAW%20New%20User%20Overview%20Summer%202016.mp4",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "224",
            "friendlyName": "Required & Mandatory Trainings",
            "category": "Training",
            "track": "CVM",
            "description": "Required & Mandatory Trainings",
            "link": "https://microsoft.sharepoint.com/sites/infopedia/pages/training.aspx",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "225",
            "friendlyName": "How to use Kustos",
            "category": "Training",
            "track": "CVM",
            "description": "How to use Kustos",
            "link": "https://microsoft.sharepoint.com/:v:/t/sbvvoicemailandasonline/EVFjsfoiGAxPr2HhBF38IbkB9zk--nDZUe9eoCeY22VbaQ?e=F7NPgw",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "226",
            "friendlyName": "How to use Torus Powershell",
            "category": "Training",
            "track": "CVM",
            "description": "How to use Torus Powershell",
            "link": "https://microsoft.sharepoint.com/:v:/t/sbvvoicemailandasonline/ETMpVjxyPwZIhPG5knXZUFoBI42dyFM7mBmdi4S4jskqbQ?e=yOLOFy",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "227",
            "friendlyName": "CQ Design & basic Investigation",
            "category": "Training",
            "track": "CVM",
            "description": "CQ Design & basic Investigation",
            "link": "https://microsoft-my.sharepoint-df.com/:v:/p/gauravsa/EV5aKJueJsJGu3qBKXFKjbgB_i7Di3nYnpRPzypgMgjGJw",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "228",
            "friendlyName": "Common IC3 Voice Applications Team Trainings",
            "category": "Training",
            "track": "CVM",
            "description": "Common IC3 Voice Applications Team Trainings",
            "link": "https://msit.microsoftstream.com/group/1fbb251c-8cfd-428d-873c-3f5bdd390607?view=videos",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "229",
            "friendlyName": "Call Queue Architeture Overview",
            "category": "Training",
            "track": "CVM",
            "description": "Call Queue Architeture Overview",
            "link": "https://microsoft.sharepoint.com/:v:/t/sbvvoicemailandasonline/EW86fxADsdtBiqSc4Xm0HF0BDVrqlYZGtT81FLPmXm6Pgg?e=fT8Uwj",
            "imgPath": "CVM.png",
            "type": "tool"
        },
        {
            "id": "230",
            "friendlyName": "Call Queue Conference Mode",
            "category": "Training",
            "track": "CVM",
            "description": "Call Queue Conference Mode",
            "link": "https://msit.microsoftstream.com/video/c1dfa3ff-0400-a520-bdf1-f1eadcdcf4c4",
            "imgPath": "CVM.png",
            "type": "tool"
        }
    ],
    "cvmSop": [
        {
            "id": "231",
            "friendlyName": "Toll Free No.Assigned to RA usage and limitation information",
            "category": "Training",
            "track": "CVM",
            "description": " Toll Free No.Assigned to RA usage and limitation information",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/9585/AA-not-answering-when-Toll-Free-Number-is-assigned-to-associated-RA  ",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "232",
            "friendlyName": "Unable to Delete Default Emergency Location associated to Tenant",
            "category": "Training",
            "track": "CVM",
            "description": " Unable to Delete Default Emergency Location associated to Tenant",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/9695/Unable-to-delete-default-emergency-location-associated-to-the-Tenant",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "233",
            "friendlyName": "Telephone No.Activation Issues in TAC",
            "category": "Training",
            "track": "CVM",
            "description": " Telephone No.Activation Issues in TAC",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11413/Telephone-Number-Activation-Issues-Showing-Incomplete-Update-Failed-Assigned-Failed-in-TAC",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "234",
            "friendlyName": "Customer Preliminary Investigation to lookup User and Tenant Information",
            "category": "Training",
            "track": "CVM",
            "description": " Customer Preliminary Investigation to lookup User and Tenant Information",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11415/Customer-Preliminary-Investigation-to-Lookup-User-or-Tenant-Information",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "235",
            "friendlyName": "Dial By Name is not working in Teams Auto Attendant",
            "category": "Training",
            "track": "CVM",
            "description": " Dial By Name is not working in Teams Auto Attendant",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11416/Dial-by-name-is-not-working-in-Teams-Auto-Attendant",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "236",
            "friendlyName": "Basic TroubleShooting steps for Dynamic Emergency Calling",
            "category": "Training",
            "track": "CVM",
            "description": " Basic TroubleShooting steps for Dynamic Emergency Calling",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/6699/Basic-TroubleShooting-steps-for-Dynamic-Emergency-calling",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "237",
            "friendlyName": "Teams Resource Accounts for AA and CQ",
            "category": "Training",
            "track": "CVM",
            "description": " Teams Resource Accounts for AA and CQ",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/3277/Teams-Resource-Accounts-for-AA-and-CQ",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "238",
            "friendlyName": "View Calls for Tenant Auto Attendant",
            "category": "Training",
            "track": "CVM",
            "description": "View Calls for Tenant Auto Attendant",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/8381/Kusto-Queries",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "239",
            "friendlyName": "NGC Resource Account Cannot be Associated with AA or CQ",
            "category": "Training",
            "track": "CVM",
            "description": "NGC Resource Account Cannot be Associated with AA or CQ",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/3283/TSG-NGC-Resource-Account-Cannot-be-Associated-with-AA-or-CQ?anchor=check-the-provisioning-status-of-the-application-instance-resource-account-in-sfb-online-using-torus",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "240",
            "friendlyName": "NGC AA and CQ: SfB Hybrid Requires Resource Accounts Created On-Premise",
            "category": "Training",
            "track": "CVM",
            "description": "NGC AA and CQ: SfB Hybrid Requires Resource Accounts Created On-Premise",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/218518/NGC-AA-and-CQ-SfB-Hybrid-Requires-Resource-Accounts-Created-On-Premises",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "241",
            "friendlyName": "Teams Auto Attendant Extension Dialing",
            "category": "Training",
            "track": "CVM",
            "description": " Teams Auto Attendant Extension Dialing",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/276272/Teams-Auto-Attendant-Extension-Dialing",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "242",
            "friendlyName": "Teams-TSG-for-Delays-in-Admin-Changes-and-User-PSTN-Functionality",
            "category": "Training",
            "track": "CVM",
            "description": "TS: Teams-TSG-for-Delays-in-Admin-Changes-and-User-PSTN-Functionality",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/23975/Teams-TSG-for-Delays-in-Admin-Changes-and-User-PSTN-Functionality",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "243",
            "friendlyName": "Teams Resource Accounts for AA and CQ",
            "category": "Training",
            "track": "CVM",
            "description": " Teams Resource Accounts for AA and CQ",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/3277/Teams-Resource-Accounts-for-AA-and-CQ",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "244",
            "friendlyName": "Known Issues - Call Queues",
            "category": "Training",
            "track": "CVM",
            "description": " Known Issues - Call Queues",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/4440/Known-Issues-Call-Queues",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "245",
            "friendlyName": "Known Issues - NGC AA and CQ",
            "category": "Training",
            "track": "CVM",
            "description": " Known Issues - NGC AA and CQ",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/3288/Known-Issues-NGC-AA-and-CQ",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "246",
            "friendlyName": "Tracing Call Queue Calls to Agents",
            "category": "Training",
            "track": "CVM",
            "description": "Tracing Call Queue Calls to Agents",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/8567/TSG-Tracing-Call-Queue-Calls-to-Agents",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "247",
            "friendlyName": "for Call Queues not shown",
            "category": "Training",
            "track": "CVM",
            "description": "for Call Queues not shown",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/311284/TSG-for-Call-Queues-not-shown",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "248",
            "friendlyName": "Teams Admin Center",
            "category": "Training",
            "track": "CVM",
            "description": " Teams Admin Center",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/102258/MoPo-Playbook",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "249",
            "friendlyName": "Troubleshooting related to Cloud Voicemail",
            "category": "Training",
            "track": "CVM",
            "description": " Troubleshooting related to Cloud Voicemail",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/4579/Troubleshooting-Voicemail-CloudVM",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "250",
            "friendlyName": "Voicemail Messages Not Delivered to Outlook and Don't Show in Teams Voicemail TAB",
            "category": "Training",
            "track": "CVM",
            "description": "Voicemail Messages Not Delivered to Outlook and Don't Show in Teams Voicemail TAB",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/122167/TSG-Voicmail-Messages-Not-Delivered-to-Outlook-and-Don't-Show-in-Teams-Voicemail-TAB",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "251",
            "friendlyName": "Teams Voicemail TAB Error or Blank",
            "category": "Training",
            "track": "CVM",
            "description": "Teams Voicemail TAB Error or Blank",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/3124/TSG-Teams-Voicemail-TAB-Error-or-Blank",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "252",
            "friendlyName": "Calls-from-On-Premises-User-to-Cloud-Voicemail-Fail-with-480",
            "category": "Training",
            "track": "CVM",
            "description": "Calls-from-On-Premises-User-to-Cloud-Voicemail-Fail-with-480",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/141852/TSG-Calls-from-On-Premises-User-to-Cloud-Voicemail-Fail-with-480",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "253",
            "friendlyName": "Receiving-email-notification-for-a-voice-message-on-Teams-the-email-is-blocked-for-impersonation-SPF-fail",
            "category": "Training",
            "track": "CVM",
            "description": "Receiving-email-notification-for-a-voice-message-on-Teams-the-email-is-blocked-for-impersonation-SPF-fail",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/212179/TSG-Receiving-email-notification-for-a-voice-message-on-Teams-the-email-is-blocked-for-impersonation-SPF-fail",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "254",
            "friendlyName": "Teams-Calls-go-directly-to-VoiceMail-Skype_ResultCode-181",
            "category": "Training",
            "track": "CVM",
            "description": "Teams-Calls-go-directly-to-VoiceMail-Skype_ResultCode-181",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/23854/TSG-Teams-Calls-go-directly-to-VoiceMail-Skype_ResultCode-181",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "255",
            "friendlyName": "Troubleshooting Hybrid Configuration",
            "category": "Training",
            "track": "CVM",
            "description": " Troubleshooting Hybrid Configuration",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/4821/Troubleshooting-Hybrid-Configuration",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "256",
            "friendlyName": "Release Phone No.from Deleted Resource Account",
            "category": "Training",
            "track": "CVM",
            "description": " Release Phone No.from Deleted Resource Account",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/143950/TSG-Release-Phone-Number-from-Deleted-Resource-Account",
            "imgPath": "CVM.png",
            "type": "sop"
        },
        {
            "id": "257",
            "friendlyName": "Cannot Assign a Phone No.to Resource Account",
            "category": "Training",
            "track": "CVM",
            "description": "Cannot Assign a Phone No.to Resource Account",
            "link": "https://dev.azure.com/Supportability/UC/_wiki/wikis/UC.wiki/98834/TSG-Cannot-Assign-a-Phone-Number-to-Resource-Account",
            "imgPath": "CVM.png",
            "type": "sop"
        }
    ]
};
var accessJsonData = {
    "allTracks": [
        {
            "id": "65",
            "friendlyName": "SD Onboarding Document",
            "category": "Access",
            "track": "All tracks",
            "description": "This is the onboarding guide to be used for any new joiners to refer",
            "link": "https://microsoft.sharepoint.com/:w:/r/teams/ucops/_layouts/15/Doc.aspx?sourcedoc=%7BEF8DB727-7A97-4C8F-9BAA-FF3D376CC7AB%7D&file=On%20Boarding%20guide.docx&action=default&mobileredirect=true",
            "imgPath": "Access_65.jpg"
        },
        {
            "id": "66",
            "friendlyName": "Torus and Yubikey setup",
            "category": "Access",
            "track": "All tracks",
            "description": "SOP to install torus and setting up yubikey to get access to prod enviroment",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5704/Torus-Installation",
            "imgPath": "Access_66.jpg"
        },
        {
            "id": "67",
            "friendlyName": "Myaccess",
            "category": "Access",
            "track": "All tracks",
            "description": "This url is used to user to be part of neccssory groups to delivery the daily activities",
            "link": "https://myaccess/identityiq/home.jsf",
            "imgPath": "Access_67.jpg"
        },
        {
            "id": "68",
            "friendlyName": "IDWeb",
            "category": "Access",
            "track": "All tracks",
            "description": "To add user to respective security and distrubition groups",
            "link": "https://idweb/identitymanagement/default.aspx",
            "imgPath": "Access_68.jpg"
        },
        {
            "id": "69",
            "friendlyName": "Fisma",
            "category": "Access",
            "track": "All tracks",
            "description": "These are mandatory trainings where everyone has to be completed and be complaint",
            "link": "https://docs.substrate.microsoft.net/docs/Security/Torus/Required-FISMA-training.html?uid=torus-required-fisma-training",
            "imgPath": "Access_69.jpg"
        },
        {
            "id": "280",
            "friendlyName": "Getting started with a SAW",
            "category": "ACCESS",
            "track": "All tracks",
            "description": "To check the information on how to get started with SAW",
            "link": "https://aka.ms/NewSAW",
            "imgPath": "CVM.png"
        },
        {
            "id": "281",
            "friendlyName": "Kusto",
            "category": "ACCESS",
            "track": "All tracks",
            "description": "For installation of the desktop client for Kusto",
            "link": "http://aka.ms/ke",
            "imgPath": "CVM.png"
        }
    ],
    "infra": [
        {
            "id": "64",
            "friendlyName": "OSP Portal",
            "category": "Access",
            "track": "Infra",
            "description": "To raise access in OSP portal to perform day to day activities",
            "link": "https://osp.office.net/idm/identity/access/Eligibilities",
            "imgPath": "Access_64.jpg"
        }
    ],
    "bgc": [
        {
            "id": "279",
            "friendlyName": "Background Screening",
            "category": "Access Background",
            "track": "CVM BGC",
            "description": "To check the Background Check Screening ",
            "link": "https://screening.microsoft.com/",
            "imgPath": "CVM.png"
        },
        {
            "id": "276",
            "friendlyName": "PowerForm Signer Information",
            "category": "Access Background",
            "track": "CVM BGC",
            "description": "To complete the attestation post background Check",
            "link": "https://www.docusign.net/Member/PowerFormSigning.aspx?PowerFormId=14f30d77-114d-49ad-9737-51ec7f0b0488 ",
            "imgPath": "CVM.png"
        }
    ]
};
var genericJsonData = {
    "allTracks": [
        {
            "id": "70",
            "friendlyName": "MSR -WSR Deck",
            "category": "Generic",
            "track": "All tracks",
            "description": "This URL is used to refer monthly and weekly customer decks",
            "link": "https://microsoft.sharepoint.com/teams/lyncsoc/Shared%20Documents/Forms/AllItems.aspx?viewid=5888ebde-0820-46ba-843a-f3bd5750ca02&id=%2Fteams%2Flyncsoc%2FShared%20Documents%2FMINDTREE%20SOC",
            "imgPath": "Generic_70.jpg"
        },
        {
            "id": "71",
            "friendlyName": "Sev012",
            "category": "Generic",
            "track": "All tracks",
            "description": "This is the link to check Sev012 icms where our team member is involved",
            "link": "https://microsoft.sharepoint.com/teams/ucops/_layouts/15/Doc.aspx?sourcedoc={c178ff05-31e4-4bcc-83be-940353288ea9}&action=edit&wd=target%28Severity%20Tracker.one%7Cf2a36ccb-0a08-4a14-8000-bac722e9f1c3%2FSev%200%5C%2F1%5C%2F2%20ICM%20%20Tracking%7C7bd871bd-2a1b-4b5c-b7fb-789e5f7993cc%2F%29",
            "imgPath": "Generic_71.jpg"
        },
        {
            "id": "72",
            "friendlyName": "Shift Roasters",
            "category": "Generic",
            "track": "All tracks",
            "description": "This url used to refer the shift roasters",
            "link": "https://microsoft.sharepoint.com/teams/ucops/Shared%20Documents/Forms/AllItems.aspx?FolderCTID=0x0120005CE45D5D14A91A4CBA708E506104AD9B&viewid=1069b3cc%2D8352%2D4491%2Db2be%2D9f90f61da801&id=%2Fteams%2Fucops%2FShared%20Documents%2FPAVC%2FSFB%20Mindtree%20Delivery%20team%2D%20Internal%2FLync%2DMT%2FShift%20Roster",
            "imgPath": "Generic_72.jpg"
        },
        {
            "id": "104",
            "friendlyName": "SFB Mindtree Delivery team - Internal",
            "category": "Generic",
            "track": "All tracks",
            "description": "SFB Mindtree Delivery team - Internal",
            "link": "https://nam06.safelinks.protection.outlook.com/?url=https%3A%2F%2Fmicrosoft.sharepoint.com%2Fteams%2Fucops%2FShared%2520Documents%2FForms%2FAllItems.aspx%3FFolderCTID%3D0x0120005CE45D5D14A91A4CBA708E506104AD9B%26View%3D%257B1069B3CC-8352-4491-B2BE-9F90F61DA801%257D%26viewid%3D1069b3cc-8352-4491-b2be-9f90f61da801%26id%3D%252Fteams%252Fucops%252FShared%2520Documents%252FPAVC%252FSFB%2520Mindtree%2520Delivery%2520team-%2520Internal&data=02%7C01%7Cv-uphegd%40microsoft.com%7Cf8566d0c71ab421cedaf08d866cddd77%7C72f988bf86f141af91ab2d7cd011db47%7C1%7C0%7C637372381661140809&sdata=pEdghe9Sj0zsops1kRUsGKEbN0TsS7JbNR5bFhs5%2Ffc%3D&reserved=0",
            "imgPath": "sharepoint.png"
        },
        {
            "id": "132",
            "friendlyName": "SOC Operations",
            "category": "Generic",
            "track": "All tracks",
            "description": "This url used to refer the SOC SLA's and Maintenance window",
            "link": "https://microsoft.sharepoint.com/teams/lyncsoc/SitePages/Home.aspx",
            "imgPath": "sharepoint.png"
        }
    ]
};
var dashboardJsonData = {
    "allTracks": [
        //{
        //    "id": "82",
        //    "friendlyName": "ImagingFailureAnalysis-VSO",
        //    "category": "Dashboard",
        //    "track": "All tracks",
        //    "description": "ImagingFailureAnalysis-VSO",
        //    "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/19185a25-c0fd-4c54-a5ce-563cda6c0905/ReportSection",
        //    "imgPath": "PowerBI_82.png"
        //},
        {
            "id": "87",
            "friendlyName": "GovEscorts Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "GovEscorts Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/8ad895c8-99e4-48bd-b597-fa1bc82910ce/ReportSection",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "88",
            "friendlyName": "UserMoves Tracker",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "UserMoves Tracker",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/55f9df53-e9f0-4d26-a795-966aa668d677",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "73",
            "friendlyName": "SRE ICM Proactive Dashboard",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Active SRE icm incidents with sla",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/ICM/SREIncidents",
            "imgPath": "Dashoboard_73.jpg"
        },
        {
            "id": "74",
            "friendlyName": "OCDM",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "This is the link for active ocdm ",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/OCDM_Health_Dashboard/Health_Dashboard",
            "imgPath": "Dashoboard_74.jpg"
        },
        {
            "id": "75",
            "friendlyName": "Service Status",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "This is the link for Service Status ",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/ServiceStatus/ServiceStatus",
            "imgPath": "ServiceStatus.png"
        },
        {
            "id": "76",
            "friendlyName": "SD ICM Proactive Dashboard",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Active SD icm incidents with sla",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/ICM/SDIncidents",
            "imgPath": "Dashoboard_73.jpg"
        },
        {
            "id": "85",
            "friendlyName": "Pavc_Msr_Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Pavc_Msr_Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/18a2b5f4-cb84-493a-8f7d-2e390d1ad3bb/ReportSection",
            "imgPath": "PowerBI_82.png"
        }
    ]
};
var operationJsonData = {

    "allTracks": [
        {
            "id": "42",
            "friendlyName": "Common mistakes in SD track",
            "category": "Operations",
            "track": "All tracks",
            "description": "This document will help new joiners to understand the common mistakes which can happen in imaging, patching, deployment and decomission tracks",
            "link": "https://microsoft.sharepoint.com/:w:/t/SfBMindtree/EScD_wlrweNNlEBckAKwP1oBcJXMn-Vu7uagI-1A0U9Q0A?e=6NDn1e",
            "imgPath": "Operations_42.jpg"
        },

        {
            "id": "43",
            "friendlyName": "Critical commands",
            "category": "Operations",
            "track": "All tracks",
            "description": "This document will help new joiners to understand the usage of critical commands and it also explain the scenarios when these commands can be invoked manually",
            "link": "https://microsoft.sharepoint.com/:f:/t/SfBMindtree/ErWM9QfJrnpChF8Lp0JrjY4BvXYaP8hiO-7RFgxjW3ruHw?e=2Fe3oa",
            "imgPath": "Operations_43.jpg"
        },
        {
            "id": "24",
            "friendlyName": "MSAsset",
            "category": "Operations",
            "track": "All tracks",
            "description": "To check the servers asset details via MSASSET Tool",
            "link": "https://msasset/",
            "imgPath": "Operations_24.jpg",
            "type": "tool"
        },
        {
            "id": "105",
            "friendlyName": "Link to check WSR and MSR preparation",
            "category": "Operations",
            "track": "All tracks",
            "description": "Link to check the ICMs for WSR and MSR preparation",
            "link": "https://msit.powerbi.com/groups/9af27dee-fa1f-4a9f-9bd1-6d52a8bc2405/reports/8c1df97c-2f6f-44a6-bf07-408f31c8573f/ReportSection94d26e703e7504ec857c",
            "imgPath": "Msr_106.png"
        },
        {
            "id": "106",
            "friendlyName": "Link for graphs to WSR and MSR preparation",
            "category": "Operations",
            "track": "All tracks",
            "description": "Link for graphs to prepare WSR and MSR preparation",
            "link": "https://msit.powerbi.com/groups/me/reports/3350d266-e34a-433e-b8ce-8704cb2d2bcc/ReportSection4a1fe46b80b25ac746ac",
            "imgPath": "Msr_106.png"
        }
    ],
    "infra": [
        {
            "id": "108",
            "friendlyName": "Icm's which are related to SOC",
            "category": "Operations",
            "track": "Infra",
            "description": "This is the query or link to fetch the icm's which are related to SOC",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced",
            "imgPath": "Dashoboard_73.JPG",
            "type": "tool"
        },
        {
            "id": "19",
            "friendlyName": "Orchestrator",
            "category": "Operations",
            "track": "Infra",
            "description": "This is the link where team can view the jobs which are scheduled",
            "link": "https://sfbweb/Orchestrator/OrchestratorValues",
            "imgPath": "Operations_19.jpg",
            "type": "tool"
        },
        {
            "id": "109",
            "friendlyName": "Create and Track GDCO",
            "category": "Operations",
            "track": "Infra",
            "description": "This is the link to create and Track GDCO requests",
            "link": "https://gdcoapp.trafficmanager.net/",
            "imgPath": "Operations_20.JPG",
            "type": "tool"
        },
        {
            "id": "110",
            "friendlyName": "Elevation for CORP",
            "category": "Operations",
            "track": "Infra",
            "description": "This link is used to take elevation for CORP to login to the servers",
            "link": "https://sasweb.microsoft.com/Member/Silo/5513",
            "imgPath": "SAS_Web.PNG",
            "type": "tool"
        },
        {
            "id": "111",
            "friendlyName": "Change of state",
            "category": "Operations",
            "track": "Infra",
            "description": "This URL is used to check the change of state for machine and recent deployments on the servers",
            "link": "https://msit.powerbi.com/groups/me/reports/557b68c5-6ce4-4790-808e-a450bd9a6a23/ReportSection?tenant=72f988bf-86f1-41af-91ab-2d7cd011db47&UPN=v-colop@microsoft.com",
            "imgPath": "PowerBI_82.png",
            "type": "tool"
        },
        {
            "id": "112",
            "friendlyName": "Pool health",
            "category": "Operations",
            "track": "Infra",
            "description": "This link is useful to check the pool health",
            "link": "https://dcot.cloudapp.net/PoolHealth",
            "imgPath": "dcot.PNG",
            "type": "tool"
        },
        {
            "id": "39",
            "friendlyName": "IPAM tool",
            "category": "Operations",
            "track": "Infra",
            "description": "To validate the ip address details of servers in IPAM tool",
            "link": "https://northcentralusprod.ipam.core.windows.net/",
            "imgPath": "Operations_39.jpg",
            "type": "tool"
        },
        {
            "id": "113",
            "friendlyName": "Servers MM status",
            "category": "Operations",
            "track": "Infra",
            "description": "To check the pool and servers MM status",
            "link": "https://jarvis-west.dc.ad.msft.net/dashboard?overrides=[{%22query%22:%22//*[id=%27Machine%27]%22,%22key%22:%22value%22,%22replacement%22:%22DM12A13FES02%22}]%20",
            "imgPath": "Jarvis.jpg",
            "type": "tool"
        },
        {
            "id": "114",
            "friendlyName": "Problem management tracker",
            "category": "Operations",
            "track": "Infra",
            "description": "Link to create and track the Bugs created for Problem Management",
            "link": "https://skype.visualstudio.com/SBS/_workitems",
            "imgPath": "SBS.png",
            "type": "tool"
        },
        {
            "id": "115",
            "friendlyName": "NIC connectivity status",
            "category": "Operations",
            "track": "Infra",
            "description": "Link to check NIC connectivity status",
            "link": "https://phynet.osdinfra.net/DeviceValidation/DeviceCli",
            "imgPath": "phynet.PNG",
            "type": "tool"
        }
    ],
    "infraSOP": [
        {
            "id": "107",
            "friendlyName": "TSG Links",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG which consists all the Important Helpful links",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2745/Helpful-Links",
            "imgPath": "wiki_1.jpg",
            "type": "sop"
        },
        {
            "id": "116",
            "friendlyName": "TSG: RIDS Applications",
            "category": "Operations",
            "track": "Infra",
            "description": "RIDS-Lookup-Success-Ratio-SuccessPercentage-lt-50",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/10632/TSG-RIDS-Applications(_Total)-RIDS-Lookup-Success-Ratio-SuccessPercentage-lt-50-",
            "imgPath": "wiki_10.jpg",
            "type": "sop"
        },
        {
            "id": "117",
            "friendlyName": "TSG: Reinstall MDS Diagnostics service",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: reinstall MDS Diagnostics service",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11577/TSG-reinstall-MDS-Diagnostics-service",
            "imgPath": "wiki_11.gif",
            "type": "sop"
        },
        {
            "id": "118",
            "friendlyName": "TSG: RIDS Applications",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: RIDS Applications(_Total) RIDS - Average time in (milliseconds) to lookup",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11190/TSG-RIDS-Applications(_Total)-RIDS-Average-time-in-(milliseconds)-to-lookup",
            "imgPath": "wiki_12.PNG",
            "type": "sop"
        },
        {
            "id": "119",
            "friendlyName": "TSG: UserStore ST Conf CRUD Failures",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: UserStore ST Conf CRUD Failures",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/10812/TSG-UserStore-ST-Conf-CRUD-Failures",
            "imgPath": "wiki_13.jpg",
            "type": "sop"
        },
        {
            "id": "120",
            "friendlyName": "TSG : IIS 10 Web Site is unavailable",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : IIS 10 Web Site is unavailable",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8522/TSG-IIS-10-Web-Site-is-unavailable",
            "imgPath": "wiki_18.jpg",
            "type": "sop"
        },
        {
            "id": "121",
            "friendlyName": "How to start Windows Services",
            "category": "Operations",
            "track": "Infra",
            "description": "How to start Windows Services that are stuck in Starting and Stopping State",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8091/How-to-start-Windows-Services-that-are-stuck-in-Starting-and-Stopping-State",
            "imgPath": "wiki_14.jpg",
            "type": "sop"
        },
        {
            "id": "122",
            "friendlyName": "TSG: Cluster resource group",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: Cluster resource group offline or partially online",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7989/TSG-Cluster-resource-group-offline-or-partially-online",
            "imgPath": "wiki_15.png",
            "type": "sop"
        },
        {
            "id": "123",
            "friendlyName": "TSG: Active Users In Resiliency Mode",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: Active Users In Resiliency Mode",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7895/TSG-Active-Users-In-Resiliency-Mode",
            "imgPath": "wiki_16.jpg",
            "type": "sop"
        },
        {
            "id": "124",
            "friendlyName": "TSG: UCWA Registering Endpoint Failures",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: UCWA Registering Endpoint Failures",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7692/TSG-UCWA-Registering-Endpoint-Failures",
            "imgPath": "wiki_2.jpg",
            "type": "sop"
        },
        {
            "id": "125",
            "friendlyName": "TSG: The HPQ WMI Namespace may be damaged",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: The HPQ WMI Namespace may be damaged",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/6488/TSG-The-HPQ-WMI-Namespace-may-be-damaged",
            "imgPath": "wiki_3.jpg",
            "type": "sop"
        },
        {
            "id": "126",
            "friendlyName": "MDM Heart beat Lost",
            "category": "Operations",
            "track": "Infra",
            "description": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/6487/MDM-Metrics-have-stopped-on-machine-(MDMHeartbeatLost)",
            "link": "MDM Metrics have stopped on machine(MDMHeartbeatLost)",
            "imgPath": "wiki_4.gif",
            "type": "sop"
        },
        {
            "id": "127",
            "friendlyName": "TSG : Alert for Dirty Shutdown",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Alert for Dirty Shutdown",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2646/TSG-Alert-for-Dirty-Shutdown",
            "imgPath": "wiki_6.png",
            "type": "sop"
        },
        {
            "id": "128",
            "friendlyName": "TSG : Database mirroring",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Database mirroring partners are not synchronized or Database Mirroring is not Synchronized or Database Mirror Witness is not accessible",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2664/TSG-Database-mirroring-partners-are-not-synchronized-or-Database-Mirroring-is-not-Synchronized-or-Database-Mirror-Witness-is-not-accessible",
            "imgPath": "wiki_7.jpg",
            "type": "sop"
        },
        {
            "id": "129",
            "friendlyName": "TSG : Database Mirror Witness is not accessible",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Database Mirror Witness is not accessible",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2665/TSG-Database-Mirror-Witness-is-not-accessible",
            "imgPath": "wiki_8.jpg",
            "type": "sop"
        },
        {
            "id": "130",
            "friendlyName": "TSG : Disk Drive Free Space",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Disk Drive Free Space",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2612/TSG-Disk-Drive-Free-Space",
            "imgPath": "wiki_9.jpg",
            "type": "sop"
        },
        {
            "id": "131",
            "friendlyName": "TSG : Local Time Synchronization Alert",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Local Time Synchronization Alert",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2661/TSG-Local-Time-Synchronization-Alert",
            "imgPath": "wiki_17.jpg",
            "type": "sop"
        }
    ],
    "customerBugSop": [
        {
            "id": "133",
            "friendlyName": "Change OnPremLineURI",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "TSG: Unable to change OnPremLineURI of meeting room account",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7740/TSG-Unable-to-change-OnPremLineURI-of-meeting-room-account",
            "imgPath": "wiki_1.jpg",
            "type": "sop"
        },
        {
            "id": "134",
            "friendlyName": "TSG : Remove On",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "TSG : Remove On - prem Line URI from User Account",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7483/TSG-Remove-On-prem-Line-URI-from-User-Account",
            "imgPath": "wiki_2.jpg",
            "type": "sop"
        },
        {
            "id": "135",
            "friendlyName": "TSG: Ported Numbers aren't working properly",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "TSG: Ported Numbers aren't working properly",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/7234/TSG-Ported-Numbers-arent-working-properly",
            "imgPath": "wiki_3.jpg",
            "type": "sop"
        },
        {
            "id": "136",
            "friendlyName": "TSG : Clear phone number from user for migration",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "TSG : Clear phone number from user for migration",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/6542/TSG-Clear-phone-number-from-user-for-migration",
            "imgPath": "wiki_4.gif",
            "type": "sop"
        },
        {
            "id": "137",
            "friendlyName": "TSG: User showing failed provisioning",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "TSG: User showing failed provisioning",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5397/TSG-User-showing-failed-provisioning",
            "imgPath": "wiki_5.PNG",
            "type": "sop"
        },
        {
            "id": "138",
            "friendlyName": "User with UserRoutingGroupId and no Pool",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "User with UserRoutingGroupId and no Pool",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5399/User-with-UserRoutingGroupId-and-no-Pool",
            "imgPath": "wiki_6.png",
            "type": "sop"
        },
        {
            "id": "139",
            "friendlyName": "Customer Bugs Preliminary Investigations",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "Customer Bugs Preliminary Investigations",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5322/Customer-Bugs-Preliminary-Investigations",
            "imgPath": "wiki_7.jpg",
            "type": "sop"
        },
        {
            "id": "140",
            "friendlyName": "[TSG - 001] Dial Pad Missing",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "[TSG - 001] Dial Pad Missing",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5270/-TSG-001-Dial-Pad-Missing",
            "imgPath": "wiki_5.PNG",
            "type": "sop"
        }
    ],
    "patchingAndDeployment": [
        {
            "id": "18",
            "friendlyName": "SFB ICMs",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This is the query or link to fetch the icm's which are related to SFB service delivery team",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=hhftndskt2t",
            "imgPath": "Operations_18.jpg",
            "type": "tool"
        },
        {
            "id": "19",
            "friendlyName": "Orchestrator",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This is the link where team can schedule the jobs via orchestrator",
            "link": "https://sfbweb/Orchestrator/OrchestratorValues",
            "imgPath": "Operations_19.jpg",
            "type": "tool"
        },
        {
            "id": "20",
            "friendlyName": "GDCO",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Access is needed to this tool to raise DCS tickets for hardware issues",
            "link": "https://gdcoapp.trafficmanager.net/requests",
            "imgPath": "Operations_20.jpg",
            "type": "tool"
        },
        {
            "id": "21",
            "friendlyName": "CORP Elevation Access",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This link is used to take elevation for CORP to login to the servers",
            "link": "https://sasweb.microsoft.com/Member",
            "imgPath": "Operations_21.jpg",
            "type": "tool"
        },
        {
            "id": "22",
            "friendlyName": "Machine state check",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This URL is used to check the change of state for machine and we can fetch the details for 60 days of data",
            "link": "https://msit.powerbi.com/groups/me/dashboards/d2d7124b-85a0-4dec-9dbe-eb0702ae0983",
            "imgPath": "Operations_22.jpg",
            "type": "tool"
        },
        {
            "id": "77",
            "friendlyName": "Active H/W ICM",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Active H/W icm incidents",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=pdlym0uoxv5",
            "imgPath": "Operations_18.jpg"
        },
        {
            "id": "78",
            "friendlyName": "ITAR ICM Queue",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "ITAR ICM Queue incidents",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=nde11gvsdo0",
            "imgPath": "Operations_18.jpg"
        },

        {
            "id": "79",
            "friendlyName": "CRC Query",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "CRC which are needed to be closed",
            "link": "https://skype.visualstudio.com/SKYPECENTRAL/_queries?tempQueryId=a5d54c17-6f71-40d8-abc7-413aa7bd93d9",
            "imgPath": "CRC_79.PNG"
        },
        {
            "id": "80",
            "friendlyName": "Dedicated Query",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Dedicated moves CRC query",
            "link": "https://skype.visualstudio.com/SKYPECENTRAL/_queries?tempQueryId=3f5bb8f0-5e99-4664-b03d-c69c344b0eea",
            "imgPath": "Dedicate_80.jpg"
        },
        {
            "id": "89",
            "friendlyName": "ITAR Shift Schedule",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "ITAR Shift Schedule details",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3153/Schedules",
            "imgPath": "ITR_Sche_89.png"
        },
        {
            "id": "81",
            "friendlyName": "Highlight, lowlight & ADHOC activities",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Highlight, lowlight & ADHOC activities",
            "link": "https://microsoft.sharepoint.com/:o:/r/teams/ucops/_layouts/15/WopiFrame.aspx?sourcedoc=%7Bc178ff05-31e4-4bcc-83be-940353288ea9%7D&action=edit&wd=target%28Highlights%20and%20Lowlights%2Eone%7C6C3F680D%2DC98A%2D4F03%2D9F52%2D9ECE1698596E%2F%29",
            "imgPath": "adhoc_81.PNG"
        },
        {
            "id": "90",
            "friendlyName": "ITAR ICM template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "ITAR ICM template",
            "link": "HTTPS://aka.ms/TeamsEscortICM",
            "imgPath": "Operations_18.jpg"
        },
        {
            "id": "275",
            "friendlyName": "Escort Teams config Sign Template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Escort Teams config Sign Template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=up353q",
            "imgPath": "Dashoboard_73.JPG"
        },
        {
            "id": "276",
            "friendlyName": "Escort Teams App rollout Template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Escort Teams App rollout Template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=h2c1Ba",
            "imgPath": "Dashoboard_73.JPG"
        },
        {
            "id": "277",
            "friendlyName": "SD to Escort ICM template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "SD to Escort ICM template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=j2ZJ1J",
            "imgPath": "Dashoboard_73.JPG"
        },

        {
            "id": "85",
            "friendlyName": "Pavc_Msr_Report",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Pavc_Msr_Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/18a2b5f4-cb84-493a-8f7d-2e390d1ad3bb/ReportSection",
            "imgPath": "PowerBI_82.png"
        },

        {
            "id": "87",
            "friendlyName": "GovEscorts Report",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "GovEscorts Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/8ad895c8-99e4-48bd-b597-fa1bc82910ce/ReportSection",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "94",
            "friendlyName": "Corp PowerBi Patching Report Link",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Corp PowerBi Patching Report Link",
            "link": "https://msit.powerbi.com/groups/me/reports/7add9d91-5825-4393-a625-fbb78549d079/ReportSection3",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "95",
            "friendlyName": "Gallatin Elevation Request Templete",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Gallatin Elevation Request Templete",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?ttl=Elevation%20request%20to%20Gallatin%20Environment,&ds=Need%20access%20to%20Gallatin%20Lync%200K,%200G%20and%20MGT1%20forest%20for%20patching%20and%20deployments.%20We%20may%20also%20request%20for%20an%20elevation%20to%20check%20certain%20things.%20%0A%0APs:%20ICM%20Template.%0A&kw=&is=20674&ic=60499&os=20674&tm=31661&sev=3&it=Deployment&env=PROD&dc=Gallatin&ci=2&ics=&sti=&spi=&cn=&sr=2&src=old",
            "imgPath": "Template_95.jpg"
        },
        {
            "id": "101",
            "friendlyName": "Gallatin Patching SOP",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Gallatin Patching SOP",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5366/Gallatin-patching",
            "imgPath": "Patch.png"
        },
        {
            "id": "92",
            "friendlyName": "Automation help for IDC team",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Automation help for IDC team",
            "link": "https://skype.visualstudio.com/SBS/_workitems/edit/1880267",
            "imgPath": "ITR_Sche_89.PNG"
        },
        {
            "id": "94",
            "friendlyName": "DCOT portal",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "DCOT portal",
            "link": "https://dcot.cloudapp.net/PoolHealth?poolName=000dco1l60plb.redmond.corp.microsoft.com&hours=4",
            "imgPath": "Generic_70.jpg"
        },
        {
            "id": "102",
            "friendlyName": "Fix_Fabric Unhealthy SOP",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Fix_Fabric Unhealthy SOP",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5305/Fix_Fabric-Unhealthy",
            "imgPath": "Corp_100.PNG"
        },
        {
            "id": "103",
            "friendlyName": "Mitigation Codes(Aka MitKeys) SOP",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Mitigation Codes(Aka MitKeys) SOP",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3350/Mitigation-Codes-(Aka-MitKeys)",
            "imgPath": "Corp_100.PNG"
        }

    ],
    "patchingAndDeploymentSop": [

        {
            "id": "44",
            "friendlyName": "Patching & Deployment Operations 1",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "ADS Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8555/ADS-Patching-SOP",
            "imgPath": "Operations_44.jpg",
            "type": "sop"
        },
        {
            "id": "45",
            "friendlyName": "Patching & Deployment Operations 2",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "CMS Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8562/CMS-Patching-Deployment-SOP",
            "imgPath": "Operations_45.jpg",
            "type": "sop"
        },
        {
            "id": "46",
            "friendlyName": "Patching & Deployment Operations 3",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "EDGE Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8385/EDGE-Patching-Deployment-SOP",
            "imgPath": "Operations_46.jpg",
            "type": "sop"
        },
        {
            "id": "47",
            "friendlyName": "Patching & Deployment Operations 4",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "VUT/SCB/SCF/FIL/LOG Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8609/VUT-Patching-Deployment-SOP",
            "imgPath": "Operations_47.jpg",
            "type": "sop"
        },
        {
            "id": "48",
            "friendlyName": "Patching & Deployment Operations 5",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "TAD Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8559/TAD-Patching-Deployment-SOP",
            "imgPath": "Operations_48.jpg",
            "type": "sop"
        },
        {
            "id": "49",
            "friendlyName": "Patching & Deployment Operations 6",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Userpool Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8496/USERPOOL-Patching-Deployment-SOP",
            "imgPath": "Operations_49.jpg",
            "type": "sop"
        },
        {
            "id": "50",
            "friendlyName": "Patching & Deployment Operations 7",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "AVMCAACPCMEDPMEBRVMeeting Pools Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8517/All-FOREST0R-AVM-CPC-CAA-PME-MED-Routing-Userpool-Roles",
            "imgPath": "Operations_50.jpg",
            "type": "sop"
        },
        {
            "id": "51",
            "friendlyName": "Patching & Deployment Operations 8",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "PRV Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8560/PRV-Patching-Deployment-SOP",
            "imgPath": "Operations_51.jpg",
            "type": "sop"
        },
        {
            "id": "52",
            "friendlyName": "Patching & Deployment Operations 9",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "WIT Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8514/WITNESS-Server-Patching-Deployment-SOP",
            "imgPath": "Operations_52.jpg",
            "type": "sop"
        },
        {
            "id": "53",
            "friendlyName": "Patching & Deployment Operations 10",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "WDS Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/8557/WDS-Patching-SOP",
            "imgPath": "Operations_53.jpg",
            "type": "sop"
        },
        {
            "id": "54",
            "friendlyName": "Patching & Deployment Operations 11",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Corp Non SFB Roles Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2903/How-to-Start-Corp-Non-SFB-Roles-Patching-using-VSTS",
            "imgPath": "Operations_54.jpg",
            "type": "sop"
        },

        {
            "id": "55",
            "friendlyName": "Patching & Deployment Operations 12",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Corp CU Update using VSTS",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2902/How-to-Start-Corp-CU-Update-using-VSTS",
            "imgPath": "Operations_55.jpg",
            "type": "sop"
        },
        {
            "id": "56",
            "friendlyName": "Patching & Deployment Operations 13",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Start Corp patching using VSTS",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2901/How-to-Start-Corp-Patching-using-VSTS",
            "imgPath": "Operations_56.jpg",
            "type": "sop"
        },
        {
            "id": "57",
            "friendlyName": "Patching & Deployment Operations 14",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "WUS Role for patching",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5613/Toolbox-Patching",
            "imgPath": "Operations_57.jpg",
            "type": "sop"
        },
        {
            "id": "93",
            "friendlyName": "SBS Orchestrator Link",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "SBS Orchestrator Link",
            "link": "https://qmweb/Orchestrator/OrchestratorValues",
            "imgPath": "SBS_93.png"

        },
        {
            "id": "100",
            "friendlyName": "Corp Patching / Deployment SOP",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Corp Patching / Deployment SOP",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2899/CORP-Patching-Deployment",
            "imgPath": "Corp_100.PNG"
        }

    ],

    "decommission": [
        {
            "id": "58",
            "friendlyName": "Operations Decommission",
            "category": "Operations",
            "track": "Decommission",
            "description": "Wiki for Physical decomission of servers",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2983/Physical-Decommission",
            "imgPath": "Operations_58.jpg"
        },
        {
            "id": "34",
            "friendlyName": "Decommission Tracker",
            "category": "Operations",
            "track": "Decommission",
            "description": "This tracker will be maintained on monthly basis to track the servers which are decomissioned",
            "link": "https://microsoft.sharepoint.com/:x:/r/teams/ucops/_layouts/15/Doc.aspx?sourcedoc=%7B44194CD2-2D5B-4A80-93B3-1017910D1EED%7D&file=MT-Decom%20Servers%20Tracking.xlsx&action=default&mobileredirect=true&cid=5fd3d114-755f-4bf8-87c2-6eef4047f3f4",
            "imgPath": "Operations_34.jpg"
        },
        {
            "id": "86",
            "friendlyName": "SFB Decommission Report",
            "category": "Operations",
            "track": "Decommission",
            "description": "SFB Decom Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/5cb3deda-ce3f-4b67-bb9e-db14101e62c4/ReportSection2",
            "imgPath": "PowerBI_82.png"
        }
    ],
    "imaging": [
        {
            "id": "24",
            "friendlyName": "MSAsset",
            "category": "Operations",
            "track": "Imaging",
            "description": "To check the servers asset details via MSASSET Tool",
            "link": "https://msasset/",
            "imgPath": "Operations_24.jpg",
            "type": "tool"
        },
        {
            "id": "30",
            "friendlyName": "Imaging Failures",
            "category": "Operations",
            "track": "Imaging",
            "description": "This is the query or link to fetch the failures which are related to imaging",
            "link": "https://skype.visualstudio.com/DefaultCollection/SBS/SFBMTD/_queries/query/?tempQueryId=97186941-42c7-416f-82c8-0b62ad029390",
            "imgPath": "Operations_30.jpg",
            "type": "tool"
        },
        {
            "id": "38",
            "friendlyName": "Lync Online Network Info",
            "category": "Operations",
            "track": "Imaging",
            "description": "To check Lync online network information",
            "link": "https://microsoft.sharepoint.com/teams/LCSLab/_layouts/15/Doc.aspx?sourcedoc={4de1cc1c-5505-4de1-91e0-a931048ae943}&action=edit&wd=target%28LO%20Network%20Info.one%7C0a41dc6e-b621-4d43-a534-74540ba0d4b6%2FSN2%20PROD%7C72071815-39b2-4be3-92b2-83baa26ce947%2F%29",
            "imgPath": "Operations_38.jpg",
            "type": "tool"
        },
        //{
        //    "id": "39",
        //    "friendlyName": "IPAM tool",
        //    "category": "Operations",
        //    "track": "Imaging",
        //    "description": "To validate the ip address details of servers in IPAM tool",
        //    "link": "https://northcentralusprod.ipam.core.windows.net/",
        //    "imgPath": "Operations_39.jpg",
        //    "type": "tool"
        //},
        {
            "id": "99",
            "friendlyName": "Imaging Tracker",
            "category": "Operations",
            "track": "Imaging",
            "description": "Imaging Tracker",
            "link": "https://nam06.safelinks.protection.outlook.com/ap/x-59584e83/?url=https%3A%2F%2Fmicrosoft.sharepoint.com%2F%3Ax%3A%2Fr%2Fteams%2Fucops%2F_layouts%2F15%2Fdoc.aspx%3Fsourcedoc%3D%257B705bbf44-7350-4f71-9b44-abece796ebd3%257D%26action%3Ddefault%26uid%3D%257B705BBF44-7350-4F71-9B44-ABECE796EBD3%257D%26ListItemId%3D19501%26ListId%3D%257B538FFC88-34B9-4FD9-8064-099F5E696FFF%257D%26odsp%3D1%26env%3Dprodbubble%26cid%3D00e2bd6d-a26c-43e0-8699-8eb79b72f8ac&data=02%7C01%7Cv-uphegd%40microsoft.com%7C43a9413a67e942e8c90c08d865bce90b%7C72f988bf86f141af91ab2d7cd011db47%7C1%7C0%7C637371209328840885&sdata=%2BMg3SX4LNjlml9JR1xybx%2FwNxZc06Z5K0dOC2xqyasA%3D&reserved=0",
            "imgPath": "Operations_39.jpg",
            "type": "tool"
        },
        {
            "id": "83",
            "friendlyName": "Imaging Metrics Report",
            "category": "Operations",
            "track": "Imaging",
            "description": "Imaging Metrics Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/cbeb481e-0c14-4bf7-9052-ace8fa13d14c/ReportSection1",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "96",
            "friendlyName": "OneAsset link",
            "category": "Operations",
            "track": "Imaging",
            "description": "OneAsset",
            "link": "https://oneasset.microsoft.com/Query",
            "imgPath": "OneAsset_96.PNG"
        },
        {
            "id": "96",
            "friendlyName": "Imaging QC",
            "category": "Operations",
            "track": "Imaging",
            "description": "BuildGates QC Result Repository",
            "link": "https://nam06.safelinks.protection.outlook.com/?url=https%3A%2F%2Fmicrosoft.sharepoint.com%2Fteams%2Fucops%2FShared%2520Documents%2FForms%2FAllItems.aspx%3FFolderCTID%3D0x0120005CE45D5D14A91A4CBA708E506104AD9B%26View%3D%257B1069B3CC-8352-4491-B2BE-9F90F61DA801%257D%26viewid%3D1069b3cc-8352-4491-b2be-9f90f61da801%26id%3D%252Fteams%252Fucops%252FShared%2520Documents%252FPAVC%252FSFB%2520Mindtree%2520Delivery%2520team-%2520Internal%252FLync-MT%252FImaging%252FBuildGates%2520QC%2520Result%2520Repository&data=02%7C01%7Cv-uphegd%40microsoft.com%7Cf8566d0c71ab421cedaf08d866cddd77%7C72f988bf86f141af91ab2d7cd011db47%7C1%7C0%7C637372381661145800&sdata=YGZWmzW1sZ9v4sZO8oGJWVJQKg6z9aUgkYhcUnHnc%2FY%3D&reserved=0",
            "imgPath": "sharepoint.png"
        },
        {
            "id": "82",
            "friendlyName": "ImagingFailureAnalysis-VSO",
            "category": "Operations",
            "track": "Imaging",
            "description": "ImagingFailureAnalysis-VSO",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/19185a25-c0fd-4c54-a5ce-563cda6c0905/ReportSection",
            "imgPath": "PowerBI_82.png"
        }

    ],
    "imagingSop": [

        {
            "id": "59",
            "friendlyName": "Imaging Operations 1",
            "category": "Operations",
            "track": "Imaging",
            "description": "Wiki for MT Imaging KickOff",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/4651/MT-imaging-KickOff",
            "imgPath": "Operations_59.jpg",
            "type": "sop"
        },
        {
            "id": "60",
            "friendlyName": "Imaging Operations 2",
            "category": "Operations",
            "track": "Imaging",
            "description": "Wiki for MT Imaging QC",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/4429/MT-Imaging-QC",
            "imgPath": "Operations_60.jpg",
            "type": "sop"
        },
        {
            "id": "98",
            "friendlyName": "MT Manual WDS Configuration SOP",
            "category": "Operations",
            "track": "Imaging",
            "description": "MT Manual WDS Configuration SOP",
            "link": "https://nam06.safelinks.protection.outlook.com/?url=https%3A%2F%2Fmicrosoft.sharepoint.com%2Fteams%2FLCSLab%2F_layouts%2FOneNote.aspx%3Fid%3D%252Fteams%252FLCSLab%252FInfrastructure%252FImaging%252FLyncOnline%252FInfrastructure%2520How%2520To%2520Guide%26wd%3Dtarget%2528LO-Lync%2520MGMT%2520Imaging.one%257C0195A43B-B942-4016-85B4-10FF2686F616%252FManual%2520WDS%2520Configuration%257CCACA7807-964B-4722-AAE9-52F8A5D8D2E6%252F%2529&data=02%7C01%7Cv-uphegd%40microsoft.com%7C43a9413a67e942e8c90c08d865bce90b%7C72f988bf86f141af91ab2d7cd011db47%7C1%7C0%7C637371209328835894&sdata=MupOWRSMYpNjang7SJvuMfV%2BVJZEK%2B3Ds2BMuCjVxQw%3D&reserved=0",
            "imgPath": "Operations_100.jpg",
            "type": "sop"
        }

    ],
    "usermoves": [
        {
            "id": "32",
            "friendlyName": "Usermoves ICM queue",
            "category": "Operations",
            "track": "Usermoves",
            "description": "This is the icm query to fetch the icm's which are related to Usermoves",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=lnba0lyrmsl",
            "imgPath": "Operations_32.jpg",
            "type": "tool"
        },
        {
            "id": "36",
            "friendlyName": "Usermoves CRC ICM Template",
            "category": "Operations",
            "track": "Usermoves",
            "description": "This is the template for Xforestintraforest CRC creation and template for creating icms for various fixes like MCO validation, fixing not ready for dual sync and datamigrating issue.",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5918/UserMoves-ICM-templates",
            "imgPath": "Operations_36.jpg",
            "type": "tool"
        },
        {
            "id": "37",
            "friendlyName": "Usermoves Portal",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Portal to start Intra & Xforest moves",
            "link": "https://sfbautomation.azurewebsites.net/Home/Index",
            "imgPath": "Operations_37.jpg",
            "type": "tool"
        },
        {
            "id": "40",
            "friendlyName": "ACMS Portal",
            "category": "Operations",
            "track": "Usermoves",
            "description": "To check the deleted documetns in ACMS portal after finalization",
            "link": "https://acmsportal.services.skypeforbusiness.com/",
            "imgPath": "Operations_40.jpg",
            "type": "tool"
        },
        {
            "id": "88",
            "friendlyName": "UserMoves Tracker",
            "category": "Operations",
            "track": "Usermoves",
            "description": "UserMoves Tracker",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/55f9df53-e9f0-4d26-a795-966aa668d677",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "97",
            "friendlyName": "IntraForest Payload status check",
            "category": "Operations",
            "track": "Usermoves",
            "description": "IntraForest Payload status check",
            "link": "https://osp.office.net/dropbox/dropboxapp/search%20payload",
            "imgPath": "Access_64.jpg"
        }
    ],
    "usermovesSop": [
        {
            "id": "61",
            "friendlyName": "Intra Forest",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Intra forest usermoves",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3016/Intra-Forest-User-Moves",
            "imgPath": "Operations_61.jpg",
            "type": "sop"
        },
        {
            "id": "62",
            "friendlyName": "X Forest",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Xforest usermoves",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11804/XForest-User-Move-SOP-v3",
            "imgPath": "Operations_62.jpg",
            "type": "sop"
        },
        {
            "id": "63",
            "friendlyName": "X Forest Migration",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Xforest migration troubleshooting guide",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3742/XForest-Migration-Trouble-Shooting-Guide",
            "imgPath": "Operations_63.jpg",
            "type": "sop"
        }
    ],
    "vulnerability": [
        {
            "id": "23",
            "friendlyName": "Vulnerability",
            "category": "Operations",
            "track": "Vulnerability",
            "description": "This link is use to fetch the vulnerability report",
            "link": "https://msit.powerbi.com/groups/me/reports/a648226f-0c8a-407a-a534-97c3f5f61524/ReportSection",
            "imgPath": "Operations_23.jpg",
            "type": "tool"
        },
        {
            "id": "26",
            "friendlyName": "Vulnerability exception",
            "category": "Operations",
            "track": "Vulnerability",
            "description": "This link is used to check and create the exceptions for vulnerabilities",
            "link": "https://pavc.azurewebsites.net",
            "imgPath": "Operations_26.jpg",
            "type": "tool"
        }
    ],
    "cvm": [
        {
            "id": "200",
            "friendlyName": "NGC Call Flow Visualizer",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the NGC Call Flow",
            "link": "https://ngc.skype.net/",
            "imgPath": "CVM.png"
        },
        {
            "id": "201",
            "friendlyName": "Call Finder",
            "category": "Operations",
            "track": "CVM",
            "description": "To Check the Internal and External Calls using PSTNCC & PSTNSIP NameSpace Logging",
            "link": "https://aka.ms/callfinder",
            "imgPath": "CVM.png"
        },
        {
            "id": "202",
            "friendlyName": "Skype CLS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the CLS Logging using SFBMdsProd NameSpace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/2B6A49F6",
            "imgPath": "CVM.png"
        },
        {
            "id": "203",
            "friendlyName": "Skype LIS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Skype LIS logging using SkypeLIS, SkypeLIS worker Namespace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/CE7224AC",
            "imgPath": "CVM.png"
        },
        {
            "id": "204",
            "friendlyName": "Skype NCS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Skype NCS Logging using SkypeNCS NameSpace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/4A69E5E6",
            "imgPath": "CVM.png"
        },
        {
            "id": "205",
            "friendlyName": "Skype LGW Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To Check the Skype LGW Core Logging using SkypeCoreLGW NameSpace",
            "link": "https://jarvis-west.dc.ad.msft.net/B1B09448",
            "imgPath": "CVM.png"
        },
        {
            "id": "206",
            "friendlyName": "ACMS Portal",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the User Policies and Settings",
            "link": "https://acmsportal.services.skypeforbusiness.com/",
            "imgPath": "CVM.png"
        },
        {
            "id": "207",
            "friendlyName": "BV Health",
            "category": "Operations",
            "track": "CVM",
            "description": "To Search  MSCDR and Sip Message  ",
            "link": "https://bvhealth.trafficmanager.net/Cdr/MsCdrSearch",
            "imgPath": "CVM.png"
        },
        {
            "id": "208",
            "friendlyName": "Impact Analysis Dashboard ",
            "category": "Operations",
            "track": "CVM",
            "description": "To  analyze the current impact using Jarvis Impact Analysis Dashboard",
            "link": "https://jarvis-west.dc.ad.msft.net/dashboard/share/4345272A?overrides=%5b%7b%22query%22:%22//*%5bid='Confidence'%5d%22,%22key%22:%22value%22,%22replacement%22:%222.Medium%22%7d,%7b%22query%22:%22//*%5bid='MinTenants'%5d%22,%22key%22:%22value%22,%22replacement%22:%222%22%7d,%7b%22query%22:%22//*%5bid='MinPairs'%5d%22,%22key%22:%22value%22,%22replacement%22:%223%22%7d,%7b%22query%22:%22//*%5bid='LessDiags'%5d%22,%22key%22:%22value%22,%22replacement%22:%22no%22%7d,%7b%22query%22:%22//*%5bid='ExclProblems'%5d%22,%22key%22:%22value%22,%22replacement%22:%22BadDest%20Robot%20FraudOrLic%22%7d%5d%20",
            "imgPath": "CVM.png"
        },
        {
            "id": "209",
            "friendlyName": "CLS Cloud DashBoard",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the CLS Logging using CLS Cloud Dashboard",
            "link": "https://clsclouddashboard.cloudapp.net/",
            "imgPath": "CVM.png"
        },
        {
            "id": "210",
            "friendlyName": "BV Kustos Logs",
            "category": "Operations",
            "track": "CVM",
            "description": "For running ad-hoc MSCDR kustos queries using Web client",
            "link": "https://dataexplorer.azure.com/clusters/skypebusinessvoice/databases/BVLogs",
            "imgPath": "CVM.png"
        },
        {
            "id": "211",
            "friendlyName": "Lynx",
            "category": "Operations",
            "track": "CVM",
            "description": "To lookup the domains that is associated with Tenant",
            "link": "https://lynx.office.net/#",
            "imgPath": "CVM.png"
        },
        {
            "id": "212",
            "friendlyName": "Kustos Logs",
            "category": "Operations",
            "track": "CVM",
            "description": "For running Ad-Hoc Kustos Queries using web client",
            "link": "https://dataexplorer.azure.com/clusters/kusto.aria.microsoft.com",
            "imgPath": "CVM.png"
        },
        {
            "id": "213",
            "friendlyName": "SFB CVM ICMs",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the query or link to fetch the icm's which are related to SFB CVM",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced",
            "imgPath": "CVM.png"
        },
        {
            "id": "214",
            "friendlyName": "CVM Handled Volume PowerBI Report",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the link used to check CVM Handled Volume PowerBi report",
            "link": "https://msit.powerbi.com/groups/me/reports/404c61ae-7dbc-4aa3-9390-14aff3fa3ce9/ReportSection",
            "imgPath": "CVM.png"
        },
        {
            "id": "215",
            "friendlyName": "Orchestrator",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the link where team can check the deployment activities",
            "link": "https://sfbweb/Orchestrator/OrchestratorValues",
            "imgPath": "CVM.png"
        },
        {
            "id": "216",
            "friendlyName": "PSTN Controller Visio - Architeture ",
            "category": "Operations",
            "track": "CVM",
            "description": "For Architeture PSTN Controller Visio",
            "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/EbdYf-ga8bpMjmuIKwm_1OkBuirrLIT5Lh2EVkVkzzE-hA ",
            "imgPath": "CVM.png"
        },
        {
            "id": "217",
            "friendlyName": "SIP Proxy Visio - - Architeture ",
            "category": "Operations",
            "track": "CVM",
            "description": "For Architeture SIP Proxy Visio ",
            "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/ET0SjlErDeZBhQcVgormU0IB8k-S15HyPBRs_V2SIsJX1Q ",
            "imgPath": "CVM.png"
        },
        {
            "id": "218",
            "friendlyName": "BYOT Flows - Architeture ",
            "category": "Operations",
            "track": "CVM",
            "description": "For Architeture of SIP Proxy Visio ",
            "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/EXAH9-F4S9hBlMA2GB131L4B6UnQFtihIpvFFwaQIDN3fw ",
            "imgPath": "CVM.png"
        },
        {
            "id": "219",
            "friendlyName": "Evolved PSTN Core - Architeture ",
            "category": "Operations",
            "track": "CVM",
            "description": "For Architeture Evolved PSTN Core",
            "link": "https://microsoft.sharepoint.com/:p:/t/PSTN/EaZiukJKHyxCvz2vru8qPbYBu7VqxPRmwlfuB6Fbg0OQ_Q ",
            "imgPath": "CVM.png"
        },
        {
            "id": "220",
            "friendlyName": "Direct Routing Configuration Guide",
            "category": "Operations",
            "track": "CVM",
            "description": "This link is used to check the Direct Routing Configuration as a guide",
            "link": "https://www.audiocodes.com/media/13253/connecting-audiocodes-sbc-to-microsoft-teams-direct-routing-enterprise-model-configuration-note.pdf",
            "imgPath": "CVM.png"
        },
        {
            "id": "221",
            "friendlyName": "Licensing",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Licensing as per SKU's",
            "link": "https://microsoft-my.sharepoint-df.com/personal/mreddy_microsoft_com/_layouts/15/Doc.aspx?sourcedoc={02f54c3f-1b04-4faa-88d7-7ff4badffce1}&action=edit&wd=target%28Access%20and%20Tools.one%7C211e7d73-0ce6-421d-81d6-f61f05918f23%2FTenant%20has%20capability%20string%28s%5C%29%7Cac867c44-3a2a-4f7a-96b0-08439aa2e23b%2F%29",
            "imgPath": "CVM.png"
        },
        {
            "id": "222",
            "friendlyName": "Stack Overflow",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Question and Answers for Developers at Microsoft",
            "link": "https://stackoverflow.microsoft.com/tour",
            "imgPath": "CVM.png"
        },
        {
            "id": "223",
            "friendlyName": "Calling Applications PSTN Services Team Notebook",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the link used to store information related to CVM for Offshore CAPS Team",
            "link": "https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/_layouts/OneNote.aspx?id=%2Fpersonal%2Fv-kusud_microsoft_com%2FDocuments%2FCalling%20Applications%20_%20PSTN%20Services%20Team\r\nonenote:https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/Documents/Calling%20Applications%20_%20PSTN%20Services%20Team/",
            "imgPath": "CVM.png"
        },
        {
            "id": "224",
            "friendlyName": "Skype Business VoiceApps Champs Notebook",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the link used to  track the incidents and information related to Voice Apps.",
            "link": "https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/_layouts/OneNote.aspx?id=%2Fteams%2FSkype_Business_Voice_Vancouver%2FSiteAssets%2FSkype%20Business%20VoiceApps%20Champs%20Notebook\r\nonenote:https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/SiteAssets/Skype%20Business%20VoiceApps%20Champs%20Notebook/",
            "imgPath": "CVM.png"
        }
    ],
    //"cvmAllTracks": [
    //    {
    //        "id": "214",
    //        "friendlyName": "CVM Handled Volume PowerBI Report",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "This is the link used to check CVM Handled Volume PowerBi report",
    //        "link": "https://msit.powerbi.com/groups/me/reports/404c61ae-7dbc-4aa3-9390-14aff3fa3ce9/ReportSection",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "215",
    //        "friendlyName": "Orchestrator",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "This is the link where team can check the deployment activities",
    //        "link": "https://sfbweb/Orchestrator/OrchestratorValues",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "216",
    //        "friendlyName": "PSTN Controller Visio - Architeture ",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "For Architeture PSTN Controller Visio",
    //        "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/EbdYf-ga8bpMjmuIKwm_1OkBuirrLIT5Lh2EVkVkzzE-hA ",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "217",
    //        "friendlyName": "SIP Proxy Visio - - Architeture ",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "For Architeture SIP Proxy Visio ",
    //        "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/ET0SjlErDeZBhQcVgormU0IB8k-S15HyPBRs_V2SIsJX1Q ",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "218",
    //        "friendlyName": "BYOT Flows - Architeture ",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "For Architeture of SIP Proxy Visio ",
    //        "link": "https://microsoft.sharepoint.com/:u:/t/PSTN/EXAH9-F4S9hBlMA2GB131L4B6UnQFtihIpvFFwaQIDN3fw ",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "219",
    //        "friendlyName": "Evolved PSTN Core - Architeture ",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "For Architeture Evolved PSTN Core",
    //        "link": "https://microsoft.sharepoint.com/:p:/t/PSTN/EaZiukJKHyxCvz2vru8qPbYBu7VqxPRmwlfuB6Fbg0OQ_Q ",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "220",
    //        "friendlyName": "Direct Routing Configuration Guide",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "This link is used to check the Direct Routing Configuration as a guide",
    //        "link": "https://www.audiocodes.com/media/13253/connecting-audiocodes-sbc-to-microsoft-teams-direct-routing-enterprise-model-configuration-note.pdf",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "221",
    //        "friendlyName": "Licensing",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "To check the Licensing as per SKU's",
    //        "link": "https://microsoft-my.sharepoint-df.com/personal/mreddy_microsoft_com/_layouts/15/Doc.aspx?sourcedoc={02f54c3f-1b04-4faa-88d7-7ff4badffce1}&action=edit&wd=target%28Access%20and%20Tools.one%7C211e7d73-0ce6-421d-81d6-f61f05918f23%2FTenant%20has%20capability%20string%28s%5C%29%7Cac867c44-3a2a-4f7a-96b0-08439aa2e23b%2F%29",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "222",
    //        "friendlyName": "Stack Overflow",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "To check the Question and Answers for Developers at Microsoft",
    //        "link": "https://stackoverflow.microsoft.com/tour",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "223",
    //        "friendlyName": "Calling Applications PSTN Services Team Notebook",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "This is the link used to store information related to CVM for Offshore CAPS Team",
    //        "link": "https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/_layouts/OneNote.aspx?id=%2Fpersonal%2Fv-kusud_microsoft_com%2FDocuments%2FCalling%20Applications%20_%20PSTN%20Services%20Team\r\nonenote:https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/Documents/Calling%20Applications%20_%20PSTN%20Services%20Team/",
    //        "imgPath": "CVM.png"
    //    },
    //    {
    //        "id": "224",
    //        "friendlyName": "Skype Business VoiceApps Champs Notebook",
    //        "category": "Operations",
    //        "track": "CVM All Tracks",
    //        "description": "This is the link used to  track the incidents and information related to Voice Apps.",
    //        "link": "https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/_layouts/OneNote.aspx?id=%2Fteams%2FSkype_Business_Voice_Vancouver%2FSiteAssets%2FSkype%20Business%20VoiceApps%20Champs%20Notebook\r\nonenote:https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/SiteAssets/Skype%20Business%20VoiceApps%20Champs%20Notebook/",
    //        "imgPath": "CVM.png"
    //    }
    //]
};
var frequentlyUsedJsonData = {
    "sdfreqUsed": [

        {
            "id": "83",
            "friendlyName": "Imaging Metrics Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Imaging Metrics Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/cbeb481e-0c14-4bf7-9052-ace8fa13d14c/ReportSection1",
            "imgPath": "PowerBI_82.png"
        },

        {
            "id": "18",
            "friendlyName": "SFB ICMs",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This is the query or link to fetch the icm's which are related to SFB service delivery team",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=hhftndskt2t",
            "imgPath": "Operations_18.jpg",
            "type": "tool"
        },
        {
            "id": "82",
            "friendlyName": "ImagingFailureAnalysis-VSO",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "ImagingFailureAnalysis-VSO",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/19185a25-c0fd-4c54-a5ce-563cda6c0905/ReportSection",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "73",
            "friendlyName": "SRE ICM Proactive Dashboard",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Active SRE icm incidents with sla",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/ICM/SREIncidents",
            "imgPath": "Dashoboard_73.jpg"
        },
        {
            "id": "76",
            "friendlyName": "SD ICM Proactive Dashboard",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Active SD icm incidents with sla",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/ICM/SDIncidents",
            "imgPath": "Dashoboard_73.jpg"
        },

        {
            "id": "77",
            "friendlyName": "Active H/W ICM",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Active H/W icm incidents",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=pdlym0uoxv5",
            "imgPath": "Operations_18.jpg"
        },
        {
            "id": "78",
            "friendlyName": "ITAR ICM Queue",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "ITAR ICM Queue incidents",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=nde11gvsdo0",
            "imgPath": "Operations_18.jpg"
        },
        {
            "id": "86",
            "friendlyName": "SFB Decom Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "SFB Decom Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/5cb3deda-ce3f-4b67-bb9e-db14101e62c4/ReportSection2",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "79",
            "friendlyName": "CRC Query",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "CRC which are needed to be closed",
            "link": "https://skype.visualstudio.com/SKYPECENTRAL/_queries?tempQueryId=a5d54c17-6f71-40d8-abc7-413aa7bd93d9",
            "imgPath": "CRC_79.PNG"
        },
        {
            "id": "80",
            "friendlyName": "Dedicated Query",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Dedicated moves CRC query",
            "link": "https://skype.visualstudio.com/SKYPECENTRAL/_queries?tempQueryId=3f5bb8f0-5e99-4664-b03d-c69c344b0eea",
            "imgPath": "Dedicate_80.jpg"
        },
        {
            "id": "81",
            "friendlyName": "Highlight, lowlight & ADHOC activities",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Highlight, lowlight & ADHOC activities",
            "link": "https://microsoft.sharepoint.com/:o:/r/teams/ucops/_layouts/15/WopiFrame.aspx?sourcedoc=%7Bc178ff05-31e4-4bcc-83be-940353288ea9%7D&action=edit&wd=target%28Highlights%20and%20Lowlights%2Eone%7C6C3F680D%2DC98A%2D4F03%2D9F52%2D9ECE1698596E%2F%29",
            "imgPath": "adhoc_81.PNG"
        },

        {
            "id": "71",
            "friendlyName": "Sev012",
            "category": "Generic",
            "track": "All tracks",
            "description": "This is the link to check Sev012 icms where our team member is involved",
            "link": "https://microsoft.sharepoint.com/teams/ucops/_layouts/15/Doc.aspx?sourcedoc={c178ff05-31e4-4bcc-83be-940353288ea9}&action=edit&wd=target%28Severity%20Tracker.one%7Cf2a36ccb-0a08-4a14-8000-bac722e9f1c3%2FSev%200%5C%2F1%5C%2F2%20ICM%20%20Tracking%7C7bd871bd-2a1b-4b5c-b7fb-789e5f7993cc%2F%29",
            "imgPath": "Generic_71.jpg"
        },
        {
            "id": "74",
            "friendlyName": "OCDM",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "This is the link for active ocdm ",
            "link": "http://sfbhealthtoolkit.azurewebsites.net/OCDM_Health_Dashboard/Health_Dashboard",
            "imgPath": "Dashoboard_74.jpg"
        },
        {
            "id": "23",
            "friendlyName": "Vulnerability",
            "category": "Operations",
            "track": "Vulnerability",
            "description": "This link is use to fetch the vulnerability report",
            "link": "https://msit.powerbi.com/groups/me/reports/a648226f-0c8a-407a-a534-97c3f5f61524/ReportSection",
            "imgPath": "Operations_23.jpg",
            "type": "tool"
        },
        {
            "id": "61",
            "friendlyName": "Intra Forest",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Intra forest usermoves",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3016/Intra-Forest-User-Moves",
            "imgPath": "Operations_61.jpg",
            "type": "sop"
        },
        {
            "id": "62",
            "friendlyName": "X Forest",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Xforest usermoves",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11804/XForest-User-Move-SOP-v3",
            "imgPath": "Operations_62.jpg",
            "type": "sop"
        },
        {
            "id": "63",
            "friendlyName": "X Forest Migration",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Wiki for Xforest migration troubleshooting guide",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/3742/XForest-Migration-Trouble-Shooting-Guide",
            "imgPath": "Operations_63.jpg",
            "type": "sop"
        },
        {
            "id": "40",
            "friendlyName": "ACMS Portal",
            "category": "Operations",
            "track": "Usermoves",
            "description": "To check the deleted documetns in ACMS portal after finalization",
            "link": "https://acmsportal.services.skypeforbusiness.com/",
            "imgPath": "Operations_40.jpg",
            "type": "tool"
        },
        {
            "id": "85",
            "friendlyName": "Pavc_Msr_Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "Pavc_Msr_Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/18a2b5f4-cb84-493a-8f7d-2e390d1ad3bb/ReportSection",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "37",
            "friendlyName": "Usermoves Portal",
            "category": "Operations",
            "track": "Usermoves",
            "description": "Portal to start Intra & Xforest moves",
            "link": "https://sfbautomation.azurewebsites.net/Home/Index",
            "imgPath": "Operations_37.jpg",
            "type": "tool"
        },
        {
            "id": "32",
            "friendlyName": "Usermoves ICM queue",
            "category": "Operations",
            "track": "Usermoves",
            "description": "This is the icm query to fetch the icm's which are related to Usermoves",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced?sl=lnba0lyrmsl",
            "imgPath": "Operations_32.jpg",
            "type": "tool"
        },
        {
            "id": "36",
            "friendlyName": "Usermoves CRC ICM Template",
            "category": "Operations",
            "track": "Usermoves",
            "description": "This is the template for Xforestintraforest CRC creation and template for creating icms for various fixes like MCO validation, fixing not ready for dual sync and datamigrating issue.",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5918/UserMoves-ICM-templates",
            "imgPath": "Operations_36.jpg",
            "type": "tool"
        },
        {
            "id": "87",
            "friendlyName": "GovEscorts Report",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "GovEscorts Report",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/8ad895c8-99e4-48bd-b597-fa1bc82910ce/ReportSection",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "39",
            "friendlyName": "IPAM tool",
            "category": "Operations",
            "track": "Imaging",
            "description": "To validate the ip address details of servers in IPAM tool",
            "link": "https://northcentralusprod.ipam.core.windows.net/",
            "imgPath": "Operations_39.jpg",
            "type": "tool"
        },
        {
            "id": "38",
            "friendlyName": "Lync Online Network Info",
            "category": "Operations",
            "track": "Imaging",
            "description": "To check Lync online network information",
            "link": "https://microsoft.sharepoint.com/teams/LCSLab/_layouts/15/Doc.aspx?sourcedoc={4de1cc1c-5505-4de1-91e0-a931048ae943}&action=edit&wd=target%28LO%20Network%20Info.one%7C0a41dc6e-b621-4d43-a534-74540ba0d4b6%2FSN2%20PROD%7C72071815-39b2-4be3-92b2-83baa26ce947%2F%29",
            "imgPath": "Operations_38.jpg",
            "type": "tool"
        },
        {
            "id": "30",
            "friendlyName": "Imaging Failures",
            "category": "Operations",
            "track": "Imaging",
            "description": "This is the query or link to fetch the failures which are related to imaging",
            "link": "https://skype.visualstudio.com/DefaultCollection/SBS/SFBMTD/_queries/query/?tempQueryId=97186941-42c7-416f-82c8-0b62ad029390",
            "imgPath": "Operations_30.jpg",
            "type": "tool"
        },
        {
            "id": "88",
            "friendlyName": "UserMoves Tracker",
            "category": "Dashboard",
            "track": "All tracks",
            "description": "UserMoves Tracker",
            "link": "https://msit.powerbi.com/groups/428dc599-23d8-4d79-84d0-7f92aee6645d/reports/55f9df53-e9f0-4d26-a795-966aa668d677",
            "imgPath": "PowerBI_82.png"
        },
        {
            "id": "24",
            "friendlyName": "MSAsset",
            "category": "Operations",
            "track": "Imaging",
            "description": "To check the servers asset details via MSASSET Tool",
            "link": "https://msasset/",
            "imgPath": "Operations_24.jpg",
            "type": "tool"
        },
        {
            "id": "22",
            "friendlyName": "Machine state check",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "This URL is used to check the change of state for machine and we can fetch the details for 60 days of data",
            "link": "https://msit.powerbi.com/groups/me/dashboards/d2d7124b-85a0-4dec-9dbe-eb0702ae0983",
            "imgPath": "Operations_22.jpg",
            "type": "tool"
        },
        {
            "id": "275",
            "friendlyName": "Escort Teams config Sign Template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Escort Teams config Sign Template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=up353q",
            "imgPath": "Dashoboard_73.JPG"
        },
        {
            "id": "276",
            "friendlyName": "Escort Teams App rollout Template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "Escort Teams App rollout Template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=h2c1Ba",
            "imgPath": "Dashoboard_73.JPG"
        },
        {
            "id": "277",
            "friendlyName": "SD to Escort ICM template",
            "category": "Operations",
            "track": "Patching & Deployment",
            "description": "SD to Escort ICM template",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=j2ZJ1J",
            "imgPath": "Dashoboard_73.JPG"
        }
    ],
    "SocfreqUsed": [
        {
            "id": "108",
            "friendlyName": "Icm's which are related to SOC",
            "category": "Operations",
            "track": "Infra",
            "description": "This is the query or link to fetch the icm's which are related to SOC",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced",
            "imgPath": "Dashoboard_73.JPG",
            "type": "tool"
        },
        {
            "id": "109",
            "friendlyName": "Create and Track GDCO",
            "category": "Operations",
            "track": "Infra",
            "description": "This is the link to create and Track GDCO requests",
            "link": "https://gdcoapp.trafficmanager.net/",
            "imgPath": "Operations_20.JPG",
            "type": "tool"
        },
        {
            "id": "110",
            "friendlyName": "Elevation for CORP",
            "category": "Operations",
            "track": "Infra",
            "description": "This link is used to take elevation for CORP to login to the servers",
            "link": "https://sasweb.microsoft.com/Member/Silo/5513",
            "imgPath": "SAS_Web.PNG",
            "type": "tool"
        },
        {
            "id": "132",
            "friendlyName": "SOC Operations",
            "category": "Generic",
            "track": "All tracks",
            "description": "This url used to refer the SOC SLA's and Maintenance window",
            "link": "https://microsoft.sharepoint.com/teams/lyncsoc/SitePages/Home.aspx",
            "imgPath": "sharepoint.png"
        },
        {
            "id": "117",
            "friendlyName": "TSG: Reinstall MDS Diagnostics service",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: reinstall MDS Diagnostics service",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/11577/TSG-reinstall-MDS-Diagnostics-service",
            "imgPath": "wiki_11.gif",
            "type": "sop"
        },
        {
            "id": "125",
            "friendlyName": "TSG: The HPQ WMI Namespace may be damaged",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG: The HPQ WMI Namespace may be damaged",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/6488/TSG-The-HPQ-WMI-Namespace-may-be-damaged",
            "imgPath": "wiki_3.jpg",
            "type": "sop"
        },
        {
            "id": "127",
            "friendlyName": "TSG : Alert for Dirty Shutdown",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Alert for Dirty Shutdown",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2646/TSG-Alert-for-Dirty-Shutdown",
            "imgPath": "wiki_6.png",
            "type": "sop"
        },
        {
            "id": "129",
            "friendlyName": "TSG : Database Mirror Witness is not accessible",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Database Mirror Witness is not accessible",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2665/TSG-Database-Mirror-Witness-is-not-accessible",
            "imgPath": "wiki_8.jpg",
            "type": "sop"
        },
        {
            "id": "130",
            "friendlyName": "TSG : Disk Drive Free Space",
            "category": "Operations",
            "track": "Infra",
            "description": "TSG : Disk Drive Free Space",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/2612/TSG-Disk-Drive-Free-Space",
            "imgPath": "wiki_9.jpg",
            "type": "sop"
        },
        {
            "id": "139",
            "friendlyName": "Customer Bugs Preliminary Investigations",
            "category": "Operations",
            "track": "CustomerBug",
            "description": "Customer Bugs Preliminary Investigations",
            "link": "https://skype.visualstudio.com/SBS/_wiki/wikis/SBS.wiki/5322/Customer-Bugs-Preliminary-Investigations",
            "imgPath": "wiki_7.jpg",
            "type": "sop"
        }
    ],
    "cvmFreqUsed": [
        {
            "id": "258",
            "friendlyName": "NGC Call Flow Visualizer",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the NGC Call Flow",
            "link": "https://ngc.skype.net/",
            "imgPath": "CVM.png"
        },
        {
            "id": "259",
            "friendlyName": "Call Finder",
            "category": "Operations",
            "track": "CVM",
            "description": "To Check the Internal and External Calls using PSTNCC & PSTNSIP NameSpace Logging",
            "link": "https://aka.ms/callfinder",
            "imgPath": "CVM.png"
        },
        {
            "id": "260",
            "friendlyName": "Skype CLS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the CLS Logging using SFBMdsProd NameSpace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/2B6A49F6",
            "imgPath": "CVM.png"
        },
        {
            "id": "261",
            "friendlyName": "Skype LIS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Skype LIS logging using SkypeLIS, SkypeLIS worker Namespace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/CE7224AC",
            "imgPath": "CVM.png"
        },
        {
            "id": "262",
            "friendlyName": "Skype NCS Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the Skype NCS Logging using SkypeNCS NameSpace in Jarvis",
            "link": "https://jarvis-west.dc.ad.msft.net/4A69E5E6",
            "imgPath": "CVM.png"
        },
        {
            "id": "263",
            "friendlyName": "Skype LGW Logging",
            "category": "Operations",
            "track": "CVM",
            "description": "To Check the Skype LGW Core Logging using SkypeCoreLGW NameSpace",
            "link": "https://jarvis-west.dc.ad.msft.net/B1B09448",
            "imgPath": "CVM.png"
        },
        {
            "id": "264",
            "friendlyName": "ACMS Portal",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the User Policies and Settings",
            "link": "https://acmsportal.services.skypeforbusiness.com/",
            "imgPath": "CVM.png"
        },
        {
            "id": "265",
            "friendlyName": "BV Health",
            "category": "Operations",
            "track": "CVM",
            "description": "To Search  MSCDR and Sip Message  ",
            "link": "https://bvhealth.trafficmanager.net/Cdr/MsCdrSearch",
            "imgPath": "CVM.png"
        },
        {
            "id": "266",
            "friendlyName": "Impact Analysis Dashboard ",
            "category": "Operations",
            "track": "CVM",
            "description": "To  analyze the current impact using Jarvis Impact Analysis Dashboard",
            "link": "https://jarvis-west.dc.ad.msft.net/dashboard/share/4345272A?overrides=%5b%7b%22query%22:%22//*%5bid='Confidence'%5d%22,%22key%22:%22value%22,%22replacement%22:%222.Medium%22%7d,%7b%22query%22:%22//*%5bid='MinTenants'%5d%22,%22key%22:%22value%22,%22replacement%22:%222%22%7d,%7b%22query%22:%22//*%5bid='MinPairs'%5d%22,%22key%22:%22value%22,%22replacement%22:%223%22%7d,%7b%22query%22:%22//*%5bid='LessDiags'%5d%22,%22key%22:%22value%22,%22replacement%22:%22no%22%7d,%7b%22query%22:%22//*%5bid='ExclProblems'%5d%22,%22key%22:%22value%22,%22replacement%22:%22BadDest%20Robot%20FraudOrLic%22%7d%5d%20",
            "imgPath": "CVM.png"
        },
        {
            "id": "267",
            "friendlyName": "CLS Cloud DashBoard",
            "category": "Operations",
            "track": "CVM",
            "description": "To check the CLS Logging using CLS Cloud Dashboard",
            "link": "https://clsclouddashboard.cloudapp.net/",
            "imgPath": "CVM.png"
        },
        {
            "id": "268",
            "friendlyName": "BV Kustos Logs",
            "category": "Operations",
            "track": "CVM",
            "description": "For running ad-hoc MSCDR kustos queries using Web client",
            "link": "https://dataexplorer.azure.com/clusters/skypebusinessvoice/databases/BVLogs",
            "imgPath": "CVM.png"
        },
        {
            "id": "269",
            "friendlyName": "Lynx",
            "category": "Operations",
            "track": "CVM",
            "description": "To lookup the domains that is associated with Tenant",
            "link": "https://lynx.office.net/#",
            "imgPath": "CVM.png"
        },
        {
            "id": "270",
            "friendlyName": "Kustos Logs",
            "category": "Operations",
            "track": "CVM",
            "description": "For running Ad-Hoc Kustos Queries using web client",
            "link": "https://dataexplorer.azure.com/clusters/kusto.aria.microsoft.com",
            "imgPath": "CVM.png"
        },
        {
            "id": "271",
            "friendlyName": "SFB CVM ICMs",
            "category": "Operations",
            "track": "CVM",
            "description": "This is the query or link to fetch the icm's which are related to SFB CVM",
            "link": "https://portal.microsofticm.com/imp/v3/incidents/search/advanced",
            "imgPath": "CVM.png"
        },
        {
            "id": "272",
            "friendlyName": "Licensing",
            "category": "Operations",
            "track": "CVM All Tracks",
            "description": "To check the Licensing as per SKU's",
            "link": "https://microsoft-my.sharepoint-df.com/personal/mreddy_microsoft_com/_layouts/15/Doc.aspx?sourcedoc={02f54c3f-1b04-4faa-88d7-7ff4badffce1}&action=edit&wd=target%28Access%20and%20Tools.one%7C211e7d73-0ce6-421d-81d6-f61f05918f23%2FTenant%20has%20capability%20string%28s%5C%29%7Cac867c44-3a2a-4f7a-96b0-08439aa2e23b%2F%29",
            "imgPath": "CVM.png"
        },
        {
            "id": "273",
            "friendlyName": "Calling Applications PSTN Services Team Notebook",
            "category": "Operations",
            "track": "CVM All Tracks",
            "description": "This is the link used to store information related to CVM for Offshore CAPS Team",
            "link": "https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/_layouts/OneNote.aspx?id=%2Fpersonal%2Fv-kusud_microsoft_com%2FDocuments%2FCalling%20Applications%20_%20PSTN%20Services%20Team\r\nonenote:https://microsoft-my.sharepoint.com/personal/v-kusud_microsoft_com/Documents/Calling%20Applications%20_%20PSTN%20Services%20Team/",
            "imgPath": "CVM.png"
        },
        {
            "id": "274",
            "friendlyName": "Skype Business VoiceApps Champs Notebook",
            "category": "Operations",
            "track": "CVM All Tracks",
            "description": "This is the link used to  track the incidents and information related to Voice Apps.",
            "link": "https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/_layouts/OneNote.aspx?id=%2Fteams%2FSkype_Business_Voice_Vancouver%2FSiteAssets%2FSkype%20Business%20VoiceApps%20Champs%20Notebook\r\nonenote:https://microsoft.sharepoint.com/teams/Skype_Business_Voice_Vancouver/SiteAssets/Skype%20Business%20VoiceApps%20Champs%20Notebook/",
            "imgPath": "CVM.png"
        }
    ]
};