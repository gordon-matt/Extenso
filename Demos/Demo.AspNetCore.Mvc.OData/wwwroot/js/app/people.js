'use strict'

var ViewModel = function () {
    var self = this;

    self.apiUrl = "/odata/PersonApi";
    
    self.validator = false;

    self.id = ko.observable(0);
    self.familyName = ko.observable(null);
    self.givenNames = ko.observable(null);
    self.dateOfBirth = ko.observable(null);

    self.init = function () {
        currentSection = $("#grid-section");
        
        self.validator = $("#form-section-form").validate({
            rules: {
                FamilyName: { required: true, maxlength: 128 },
                GivenNames: { required: true, maxlength: 128 },
                DateOfBirth: { required: true, date: true }
            }
        });

        $("#grid").kendoGrid({
            data: null,
            dataSource: {
                type: 'odata-v4',
                transport: {
                    read: self.apiUrl
                },
                schema: {
                    model: {
                        fields: {
                            FamilyName: { type: "string" },
                            GivenNames: { type: "string" },
                            DateOfBirth: { type: "date" }
                        }
                    }
                },
                pageSize: 10,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                sort: [
                    { field: "FamilyName", dir: "asc" },
                    { field: "GivenNames", dir: "asc" }
                ]
            },
            dataBound: function (e) {
                var body = this.element.find("tbody")[0];
                if (body) {
                    ko.cleanNode(body);
                    ko.applyBindings(ko.dataFor(body), body);
                }
            },
            filterable: true,
            sortable: {
                allowUnsort: false
            },
            pageable: {
                refresh: true
            },
            scrollable: false,
            columns: [{
                field: "FamilyName",
                title: "Family Name"
            }, {
                field: "GivenNames",
                title: "Given Name/s"
            }, {
                field: "DateOfBirth",
                title: "Date of Birth",
                type: "date",
                format: "{0:yyyy-MM-dd}",
                filterable: false,
                width: 200
            }, {
                field: "Id",
                title: "&nbsp;",
                template:
                    '<div class="btn-group">' +
                        '<button type="button" data-bind="click: edit.bind($data,\'#=Id#\')" class="btn btn-default btn-sm" title="Edit"><i class="fa fa-edit"></i></button>' +
                        '<button type="button" data-bind="click: remove.bind($data,\'#=Id#\')" class="btn btn-danger btn-sm" title="Delete"><i class="fa fa-remove"></i></button>' +
                    '</div>',
                attributes: { "class": "text-center" },
                filterable: false,
                width: 100
            }]
        });

        $("#DateOfBirth").kendoDatePicker({
            start: "decade",
            format: "yyyy-MM-dd"
        });
    };

    self.create = function () {
        self.id(0);
        self.familyName(null);
        self.givenNames(null);
        self.dateOfBirth(null);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html("Create");
    };

    self.edit = function (id) {
        $.ajax({
            url: self.apiUrl + "(" + id + ")",
            type: "GET",
            dataType: "json",
            async: false
        })
        .done(function (json) {
            self.id(json.Id);
            self.familyName(json.FamilyName);
            self.givenNames(json.GivenNames);
            self.dateOfBirth(json.DateOfBirth);

            switchSection($("#form-section"));
            $("#form-section-legend").html("Edit");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $.notify({ message: "Error when trying to retrieve record!", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            console.log(textStatus + ': ' + errorThrown);
        });
    };

    self.remove = function (id) {
        if (confirm("Are you sure that you want to delete this record?")) {
            $.ajax({
                url: self.apiUrl + "(" + id + ")",
                type: "DELETE",
                async: false
            })
            .done(function (json) {
                self.refreshGrid();
                $.notify({ message: "Successfully deleted record!", icon: 'fa fa-check' }, { type: 'success' });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify({ message: "Error when trying to delete record!", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.save = function () {
        var isNew = (self.id() == 0);

        if (!$("#form-section-form").valid()) {
            return false;
        }

        var record = {
            Id: self.id(),
            FamilyName: self.familyName(),
            GivenNames: self.givenNames(),
            DateOfBirth: self.dateOfBirth()
        };

        if (isNew) {
            $.ajax({
                url: self.apiUrl,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refreshGrid();
                switchSection($("#grid-section"));
                $.notify({ message: "Successfully inserted record!", icon: 'fa fa-check' }, { type: 'success' });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify({ message: "Error when trying to insert record!", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                console.log(textStatus + ': ' + errorThrown);
            });
        }
        else {
            $.ajax({
                url: self.apiUrl + "(" + self.id() + ")",
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(record),
                dataType: "json",
                async: false
            })
            .done(function (json) {
                self.refreshGrid();
                switchSection($("#grid-section"));
                $.notify({ message: "Successfully updated record!", icon: 'fa fa-check' }, { type: 'success' });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $.notify({ message: "Error when trying to update record!", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                console.log(textStatus + ': ' + errorThrown);
            });
        }
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };

    self.refreshGrid = function () {
        $('#grid').data('kendoGrid').dataSource.read();
        $('#grid').data('kendoGrid').refresh();
    };
};