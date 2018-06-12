export class AssemblyViewModel {
    assemblies = [];
    selectedAssembly = null;
    selectedNamespace = null;
    selectedTypesTitle = null;
    selectedTypes = [];
    selectedType = null;

    constructor() {
        let self = this;

        $.ajax({
            dataType: "json",
            url: "/Extenso/js/assemblies.json",
            async: false,
            success: function (json) {
                $.each(json, function (i) {
                    self.assemblies.push(this);
                });
            }
        });
    }

    activate(params, routeConfig) {
        this.selectedAssembly = this.assemblies.find(x => x.Id == params.id);
    }

    showClasses(namespaceName) {
        this.changeNamespace(namespaceName);
        this.selectedTypesTitle = 'Classes';
        this.selectedTypes = this.selectedNamespace.Classes;
    }

    showStructures(namespaceName) {
        this.changeNamespace(namespaceName);
        this.selectedTypesTitle = 'Structures';
        this.selectedTypes = this.selectedNamespace.Structures;
    }

    showInterfaces(namespaceName) {
        this.changeNamespace(namespaceName);
        this.selectedTypesTitle = 'Interfaces';
        this.selectedTypes = this.selectedNamespace.Interfaces;
    }

    showEnumerations(namespaceName) {
        this.changeNamespace(namespaceName);
        this.selectedTypesTitle = 'Enumerations';
        this.selectedTypes = this.selectedNamespace.Enumerations;
    }

    showDelegates(namespaceName) {
        this.changeNamespace(namespaceName);
        this.selectedTypesTitle = 'Delegates';
        this.selectedTypes = this.selectedNamespace.Delegates;
    }

    changeNamespace(namespaceName) {
        this.selectedType = null;
        this.selectedTypes = [];
        this.selectedNamespace = this.selectedAssembly.Namespaces.find(x => x.Name == namespaceName);
    }

    showType(type) {
        this.selectedType = type;
        window.setTimeout(() => $('.nav-tabs a:first').tab('show'), 200);
        window.setTimeout(() => PR.prettyPrint(), 200);
    }
}