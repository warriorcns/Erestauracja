<%@ Page Title="" Language="C#" MasterPageFile="~/Views/POS/POS.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >
    
    <script src="../../Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../../Scripts/DataTables-1.9.4/media/js/jquery.js" type="text/javascript"></script>
    <script src="../../Scripts/DataTables-1.9.4/media/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <link href="../../Content/DataTables-1.9.4/media/css/demo_page.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Content/DataTables-1.9.4/media/css/demo_table.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Content/DataTables-1.9.4/media/css/jquery.dataTables.css" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" charset="utf-8">
        /* Formating function for row details */
        function fnFormatDetails(oTable, nTr) {
            var aData = oTable.fnGetData(nTr);
            var sOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
            sOut += '<tr><td>Rendering engine:</td><td>' + aData[1] + ' ' + aData[4] + '</td></tr>';
            sOut += '<tr><td>Link to source:</td><td>Could provide a link here</td></tr>';
            sOut += '<tr><td>Extra info:</td><td>And any further details here (images etc)</td></tr>';
            sOut += '</table>';

            return sOut;
        }

        $(document).ready(function () {
            /*
            * Insert a 'details' column to the table
            */
            var nCloneTh = document.createElement('th');
            var nCloneTd = document.createElement('td');
            nCloneTd.innerHTML = '<img src="../../Content/DataTables-1.9.4/media/images/details_open.png"/>';
            nCloneTd.className = "center";

            $('#example thead tr').each(function () {
                this.insertBefore(nCloneTh, this.childNodes[0]);
            });

            $('#example tbody tr').each(function () {
                this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
            });

            /*
            * Initialse DataTables, with no sorting on the 'details' column
            */
            var oTable = $('#example').dataTable({
                "aoColumnDefs": [
						{ "bSortable": false, "aTargets": [0] }
					],
                "aaSorting": [[1, 'asc']]
            });

            /* Add event listener for opening and closing details
            * Note that the indicator for showing which row is open is not controlled by DataTables,
            * rather it is done here
            */
            $('#example tbody td img').live('click', function () {
                var nTr = $(this).parents('tr')[0];
                if (oTable.fnIsOpen(nTr)) {
                    /* This row is already open - close it */
                    this.src = "../../Content/DataTables-1.9.4/media/images/details_open.png";
                    oTable.fnClose(nTr);
                }
                else {
                    /* Open this row */
                    this.src = "../../Content/DataTables-1.9.4/media/images/details_close.png";
                    oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
                }
            });
                });    
		</script>

     
     <div style="position:relative; top:50px;">
    <table cellpadding="0" cellspacing="0" border="0" class="display" id="example">
        <thead>
            <tr>
                <th>
                    Rendering engine
                </th>
                <th>
                    Browser
                </th>
                <th>
                    Platform(s)
                </th>
                <th>
                    Engine version
                </th>
                <th>
                    CSS grade
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="gradeX">
                <td>
                    Trident
                </td>
                <td>
                    Internet Explorer 4.0
                </td>
                <td>
                    Win 95+
                </td>
                <td class="center">
                    4
                </td>
                <td class="center">
                    X
                </td>
            </tr>
            <tr class="gradeC">
                <td>
                    Trident
                </td>
                <td>
                    Internet Explorer 5.0
                </td>
                <td>
                    Win 95+
                </td>
                <td class="center">
                    5
                </td>
                <td class="center">
                    C
                </td>
            </tr>
        </tbody>
    </table>
    
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
