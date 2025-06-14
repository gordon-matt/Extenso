'use strict'

class ViewModel {
    constructor() {
        this.validator = false;

        this.id = ko.observable(0);
        this.familyName = ko.observable(null);
        this.givenNames = ko.observable(null);
        this.dateOfBirth = ko.observable(null);
    }

    init = () => {
        currentSection = $("#grid-section");

        this.validator = $("#form-section-form").validate({
            rules: {
                FamilyName: { required: true, maxlength: 128 },
                GivenNames: { required: true, maxlength: 128 },
                DateOfBirth: { required: true, date: true }
            }
        });

        $("#grid").kendoGrid({
            data: null,
            dataSource: {
                transport: {
                    read: {
                        type: 'post',
                        dataType: 'json',
                        url: `/people/grid`
                    }
                },
                schema: {
                    data: 'data',
                    total: 'total',
                    model: {
                        fields: {
                            familyName: { type: "string" },
                            givenNames: { type: "string" },
                            dateOfBirth: { type: "date" }
                        }
                    }
                },
                pageSize: 10,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                sort: [
                    { field: "familyName", dir: "asc" },
                    { field: "givenNames", dir: "asc" }
                ]
            },
            dataBound: function (e) {
                let body = this.element.find("tbody")[0];
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
                field: "familyName",
                title: "Family Name"
            }, {
                field: "givenNames",
                title: "Given Name/s"
            }, {
                field: "dateOfBirth",
                title: "Date of Birth",
                type: "date",
                format: "{0:yyyy-MM-dd}",
                filterable: false,
                width: 200
            }, {
                field: "id",
                title: "&nbsp;",
                template: '<div class="btn-group">' +
                    '<button type="button" data-bind="click: edit.bind($data,\'#=id#\')" class="btn btn-default btn-sm" title="Edit"><i class="fa fa-edit"></i></button>' +
                    '<button type="button" data-bind="click: remove.bind($data,\'#=id#\')" class="btn btn-danger btn-sm" title="Delete"><i class="fa fa-remove"></i></button>' +
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

    create = () => {
        this.id(0);
        this.familyName(null);
        this.givenNames(null);
        this.dateOfBirth(null);

        this.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html("Create");
    };

    edit = async (id) => {
        const data = await ApiHelper.getRecord(`/people/${id}`);
        this.id(data.id);
        this.familyName(data.familyName);
        this.givenNames(data.givenNames);
        this.dateOfBirth(data.dateOfBirth);

        switchSection($("#form-section"));
        $("#form-section-legend").html("Edit");
    };

    remove = async (id) => {
        await ApiHelper.deleteRecord(`/people/${id}`);
    };

    save = async () => {
        const isNew = (this.id() == 0);

        if (!$("#form-section-form").valid()) {
            return false;
        }

        const record = {
            id: this.id(),
            familyName: this.familyName(),
            givenNames: this.givenNames(),
            dateOfBirth: this.dateOfBirth()
        };

        if (isNew) {
            await ApiHelper.postRecord(`/people`, record);
        }
        else {
            await ApiHelper.putRecord(`/people/${this.id()}`, record);
        }
    };

    cancel = () => {
        switchSection($("#grid-section"));
    };
}