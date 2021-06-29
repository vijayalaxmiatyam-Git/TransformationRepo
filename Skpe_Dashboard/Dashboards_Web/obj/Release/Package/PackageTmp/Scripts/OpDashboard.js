var processedJsonDataNew = "";
var dataNa = "";
var dataTool = "";
var dataWiki = "";
function SortSfbLinkData() {

    var sort = $("#sortSfbdropdown :selected").val();

    if (sort == 'select') {
        return;
    }

    else {
        $.post('/OperationalDashBoard/SfbDataSort', { sortDir: sort }, function (sortedData) {
            var stringHtml = "";
            if ($("#selDropdownCategory :selected").val() == "" || $("#selDropdownCategory :selected").val() == null) {

                document.getElementById('linksContainer').innerHTML = "";
                processedJsonDataNew = sortedData;

                for (var i = 0; i < sortedData.length; i++) {
                    stringHtml += makeStringNew(i);
                }

                document.getElementById('linksContainer').innerHTML = stringHtml;
            }
            else {

                var startToolRow = '<div class="row commontoolsopheading">Tool</div>';
                var startSOPRow = '<div class="row commontoolsopheading sopheading">Wiki</div>';
                var divClose = '</div>';
                document.getElementById('linksContainer').innerHTML = "";
                dataNa = new Array(sortedData.length);
                dataTool = new Array(sortedData.length);
                dataWiki = new Array(sortedData.length);
                var toolCount = 0;
                var wikiCount = 0;
                var naCount = 0;
                for (var i = 0; i < sortedData.length; i++) {

                    if (sortedData[i].LinkType.toLowerCase() == "tool") {
                        dataTool[toolCount] = sortedData[i];
                        toolCount++;
                    }
                    else if (sortedData[i].LinkType.toLowerCase() == "na") {
                        dataNa[naCount] = sortedData[i];
                        naCount++;
                    }
                    else {
                        dataWiki[wikiCount] = sortedData[i];
                        wikiCount++;
                    }
                }

                processedJsonDataNew = dataTool;
                if (toolCount > 0) {
                    stringHtml += startToolRow;
                    for (var i = 0; i < toolCount; i++) {
                        stringHtml += makeStringNew(i);
                    }
                    stringHtml += divClose;
                }

                processedJsonDataNew = dataWiki;
                if (wikiCount > 0) {
                    stringHtml += startSOPRow;
                    for (var i = 0; i < wikiCount; i++) {
                        stringHtml += makeStringNew(i);
                    }
                    stringHtml += divClose;
                }

                processedJsonDataNew = dataNa;
                if (naCount > 0) {
                    for (var i = 0; i < naCount; i++) {
                        stringHtml += makeStringNew(i);
                    }
                    stringHtml += divClose;
                }

                toolCount = 0;
                wikiCount = 0;
                naCount = 0;

                dataNa = "";
                dataTool = "";
                dataWiki = "";

                document.getElementById('linksContainer').innerHTML = stringHtml;
            }
        });
    }
}

function FrequentlyUsedSelect() {
    SortDropdownReset();
    $.post('/OperationalDashBoard/GetFrequentlyUsedData', { freqId: $("#dropdownFreqUsed :selected").val() }, function (data) {
        var stringHtml = "";
        CategoryDropDownReset();
        DisableTrack();
        document.getElementById('linksContainer').innerHTML = "";
        processedJsonDataNew = data;

        for (var i = 0; i < data.length; i++) {
            stringHtml += makeStringNew(i);
        }

        document.getElementById('linksContainer').innerHTML = stringHtml;
    });
}

function CategoryDropDownReset() {
    var dropDown = document.getElementById("selDropdownCategory");
    dropDown.selectedIndex = 0;
}
function DisableTrack() {
    $('#Track').empty();
    var newOption = $('<option>Select Track</option>');
    $('#Track').append(newOption);
    $('#Track').prop("disabled", true);
}

function makeStringNew(index) {
    var result = "";
    if (index == 0) {
        result += '<div class="row rowImageContainer">';
    }
    var colCounter = index + 1;
    result += "<div class='col-lg-2 col-sm-2 col-xs-2 imageConatinerCols'>" +
        '<a  href="' + processedJsonDataNew[index].Link + '"target="_blank"> <img src="Content/img/' + processedJsonDataNew[index].ImagePath + '" class="img-thumbnail" alt="' + processedJsonDataNew[index].FriendlyName + '"></a>' +
        '<div>' +
        '<ul class="list-group list-group-flush titleDesc">' +
        '<a  href="' + processedJsonDataNew[index].Link + '"target="_blank"><li class="list-group-item titleText">' +
        processedJsonDataNew[index].FriendlyName + '</li></a>' +
        '<li class="list-group-item descText">' + processedJsonDataNew[index].Description + '<button type="button" style = "text-decoration:none; font-weight:bold; padding:0;margin-left: 4px;" class="btn btn-link" onclick="GetEditDetails(' + processedJsonDataNew[index].SfbLibId + ')" id="btnEdit" data-target="#editModal">Edit</button>' + '</li>' +
        '</ul>' +
        '</div>' +
        '</div>';
    if (colCounter % 6 == 0) {
        result += '</div>';
        result += '<div class="row rowImageContainer">';
    }
    return result;
}

function ChangeDropDown() {

    var categoryId = $("#selDropdownCategory :selected").val();
    SortDropdownReset();
    if (categoryId == "" || categoryId == null) {
        DisableTrack();
        FrequentlyUsedSelect();
        return;
    }
    $('#Track').prop("disabled", false);
    $.ajax({
        url: '/OperationalDashBoard/GetTrackByCategory',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { categoryId: categoryId },
        success: function (data) {
            $("#Track").html('');
            $.each(data, function (i, data) {
                $('<option>',
                    {
                        value: data.Id,
                        text: data.Name
                    }).html(data.Name).appendTo("#Track");
            });
            $("#trackDiv").show();

            GetDetailsByCategoryTrack();
        },
        error: function (xhr, status) {
        }
    });
}

function GetDetailsByCategoryTrack() {
    var categoryId = $("#selDropdownCategory :selected").val();
    var trackId = $("#Track :selected").val();

    $.post('/OperationalDashBoard/GetDetailsByCategoryTrack', { catId: categoryId, trackId: trackId }, function (data) {
        var stringHtml = "";
        var startToolRow = '<div class="row commontoolsopheading">Tool</div>';
        var startSOPRow = '<div class="row commontoolsopheading sopheading">Wiki</div>';
        var divClose = '</div>';
        document.getElementById('linksContainer').innerHTML = "";
        dataNa = new Array(data.length);
        dataTool = new Array(data.length);
        dataWiki = new Array(data.length);
        var toolCount = 0;
        var wikiCount = 0;
        var naCount = 0;
        for (var i = 0; i < data.length; i++) {

            if (data[i].LinkType.toLowerCase() == "tool") {
                dataTool[toolCount] = data[i];
                toolCount++;
            }
            else if (data[i].LinkType.toLowerCase() == "na") {
                dataNa[naCount] = data[i];
                naCount++;
            }
            else {
                dataWiki[wikiCount] = data[i];
                wikiCount++;
            }
        }

        processedJsonDataNew = dataTool;
        if (toolCount > 0) {
            stringHtml += startToolRow;
            for (var i = 0; i < toolCount; i++) {
                stringHtml += makeStringNew(i);
            }
            stringHtml += divClose;
        }

        processedJsonDataNew = dataWiki;
        if (wikiCount > 0) {
            stringHtml += startSOPRow;
            for (var i = 0; i < wikiCount; i++) {
                stringHtml += makeStringNew(i);
            }
            stringHtml += divClose;
        }

        processedJsonDataNew = dataNa;
        if (naCount > 0) {
            for (var i = 0; i < naCount; i++) {
                stringHtml += makeStringNew(i);
            }
            stringHtml += divClose;
        }

        toolCount = 0;
        wikiCount = 0;
        naCount = 0;

        dataNa = "";
        dataTool = "";
        dataWiki = "";

        document.getElementById('linksContainer').innerHTML = stringHtml;
    });
}

function searchSfbLinks() {
    var textFromSearchBox = document.getElementById('searchLinkName').value;
    $.post('/OperationalDashBoard/SfbDataSearch', { searchText: textFromSearchBox }, function (Data) {
        var stringHtml = "";

        document.getElementById('linksContainer').innerHTML = "";
        processedJsonDataNew = Data;

        for (var i = 0; i < Data.length; i++) {
            stringHtml += makeStringNew(i);
        }

        document.getElementById('linksContainer').innerHTML = stringHtml;
    });
}

function SortDropdownReset() {
    var dropDown = document.getElementById("sortSfbdropdown");
    dropDown.selectedIndex = 0;
}

function GetEditDetails(id) {
    $.post('/OperationalDashBoard/GetDataForEdit', { libId: id }, function (data) {
        $("#editLinkId").val(data.SfbLibId);
        $("#editLink").val(data.Link);
        $("#editName").val(data.FriendlyName);
        $("#editDescription").val(data.Description);
        $("#modTitle").text('Edit - ' + data.FriendlyName);
    });
    $("#lblEditError").text('');
    $("#lblEditSuccess").text('');
    $("#editModal").modal('show');
}

function UpdateSFBData() {
    $("#lblEditError").text('');
    $("#lblEditSuccess").text('');

    if ($("#editDescription").val() == "" || $("#editDescription").val() == null) {
        $("#lblEditError").text("Enter description");
        $("#lblEditError").show();
        return;
    }
    if ($("#editLink").val() == "" || $("#editLink").val() == null) {
        $("#lblEditError").text("Enter the Link");
        $("#lblEditError").show();
        return;
    }

    var modelData = { Description: $("#editDescription").val(), Link: $("#editLink").val(), SfbLibId: $("#editLinkId").val() };
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '/OperationalDashBoard/UpdateSFBLibraryData',
        data: JSON.stringify(modelData),
        success: function (message) {
            if (message.success == false) {
                $("#lblEditError").text(message.responseText);
                $("#lblEditError").show();
            }
            else {
                $("#lblEditSuccess").text(message.responseText);
                $("#lblEditSuccess").show();
            }
        },
        error: function (response) {

        }
    });
}
