   $(function () {


     $('#tblToolInstruments').dataTable({

          "bDestroy": true,
                "bProcessing": true,
               // "scrollX": true,
                "bServerSide": true,
                "bSortCellsTop": true,
                "sAjaxSource": '../Instrument/ToolRoomDepartmentList',
               "fnServerParams": function (aoData) { 
                     aoData.push({ "name": "sscode", "value": $('#txtsearch5').val() })
                    aoData.push({ "name": "instrumentname", "value": $('#txtsearch0').val() })
                    aoData.push({ "name": "labid", "value": $('#txtsearch2').val() })
                    aoData.push({ "name": "typeOfEquipment", "value": $('#drptypeOfEquipment').val() })
                   
                    aoData.push({ "name": "serialno", "value": $('#txtsearch1').val() })
                    aoData.push({ "name": "range", "value": $('#txtsearch3').val() })
                    aoData.push({ "name": "department", "value": $('#txtsearch4').val() })
                    aoData.push({ "name": "calibrationdate", "value": $('#txtsearch7').val() })
                    aoData.push({ "name": "duedate", "value": $('#txtsearch8').val() })
                },
            "fnServerData": function (sSource, aoData, fnCallback) {
                    $.ajax({
                        type: 'GET',
                        url: sSource,
                        data:aoData,
                        success: fnCallback
                    })
                },

                 "columns":[
                     {"data":"instrumentName","name":"Instrument Name"},
                     {"data":"slNo","name":"Serial Number"},
                     {"data":"idNo","name":"Lab ID Number"},
                     {"data":"range","name":"Range"},
                     {"data":"departmentName","name":"Department"},
                     { "data":"subSectionCode","name":"Sub section code"},
                     {"data":"typeOfEquipment","name":"Scope"},
                     {"data":"sCalibDate","name":"Calibration Date"},
                      {"data":"sDueDate","name":"DueDate"},
                  
                
                 ],
                  "createdRow": function (row, data, dataIndex) {
                      },
                         "columnDefs": [
                {
                'orderable': false,
               'targets': 0,
               'checkboxes': {
               'selectRow': true
                }
            }
             ],
             'select': {
            'style': 'multi'
          },

                // "serverside":"true",
                 "order":[0,"asc"],

                 "pageLength": "100",
                 "language": {
                    //"emptyTable": "No record found.",
                    "processing":
                        '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
                },
                 dom: "<'row'<'col-sm-3'l><'col-sm-3'f><'col-sm-6'p>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
                  "initComplete": function() {
      // Select the column whose header we need replaced using its index(0 based)
      this.api().column(6).every(function() {
          var $table = $("#tblToolInstruments");
            var title = $(this).text();
        var column = this;
            var select = $('<select style="width: 100px;" id="drptypeOfEquipment"  onchange="DrptypeOfEquipment(this.value)" class="form-control input-sm"><option selected value="">All</option></select>')
        
          .appendTo($table.find('thead tr:eq(1) th:eq(' + this.index() + ')'))
          
        column.data().unique().sort().each(function(d, j) {
            if(d !=null)
            {
                  select.append("<option value='" + d + "'>" + d + "</option>")
            }
         
        });
      });
    },


 
 })
 })


  $(document).ready(function() {


   $("#tblToolInstruments thead tr:eq(1) th").each(function (i) {
        var title1 = $(this).text();
        var title = title1.trim();
     if(i==0 ||i==1 ||i==2 || i==3|| i==4|| i==5|| i==7|| i==8)
     {
       $(this).html('<input id="txtsearch'+i+'" style="width: 100px;"  class="form-control filter" type="text" placeholder="Search" />');

     }
  
     else
     {
       $(this).html('<input  style="width: auto;display:none"  class="form-control filter" type="text" placeholder="Search" />');

     }
    });
   
        var table = $('#tblToolInstruments').DataTable();
           table.columns().every(function () {
         //var table = this;
          $('input').on('keyup change', function () {
                if (table.search() !== this.value) {
                	table.search(this.value).draw();
                  
                }
            });
        });

 

      })


      
       
    