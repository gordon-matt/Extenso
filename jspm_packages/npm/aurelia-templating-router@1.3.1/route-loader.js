/* */ 
define(['exports', 'aurelia-dependency-injection', 'aurelia-templating', 'aurelia-router', 'aurelia-path', 'aurelia-metadata', './router-view'], function (exports, _aureliaDependencyInjection, _aureliaTemplating, _aureliaRouter, _aureliaPath, _aureliaMetadata, _routerView) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.TemplatingRouteLoader = undefined;

  function _possibleConstructorReturn(self, call) {
    if (!self) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return call && (typeof call === "object" || typeof call === "function") ? call : self;
  }

  function _inherits(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function, not " + typeof superClass);
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        enumerable: false,
        writable: true,
        configurable: true
      }
    });
    if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass;
  }

  

  var _dec, _class, _dec2, _class2;

  var EmptyClass = (_dec = (0, _aureliaTemplating.inlineView)('<template></template>'), _dec(_class = function EmptyClass() {
    
  }) || _class);
  var TemplatingRouteLoader = exports.TemplatingRouteLoader = (_dec2 = (0, _aureliaDependencyInjection.inject)(_aureliaTemplating.CompositionEngine), _dec2(_class2 = function (_RouteLoader) {
    _inherits(TemplatingRouteLoader, _RouteLoader);

    function TemplatingRouteLoader(compositionEngine) {
      

      var _this = _possibleConstructorReturn(this, _RouteLoader.call(this));

      _this.compositionEngine = compositionEngine;
      return _this;
    }

    TemplatingRouteLoader.prototype.loadRoute = function loadRoute(router, config) {
      var childContainer = router.container.createChild();

      var viewModel = void 0;
      if (config.moduleId === null) {
        viewModel = EmptyClass;
      } else if (/\.html/i.test(config.moduleId)) {
        viewModel = createDynamicClass(config.moduleId);
      } else {
        viewModel = (0, _aureliaPath.relativeToFile)(config.moduleId, _aureliaMetadata.Origin.get(router.container.viewModel.constructor).moduleId);
      }

      config = config || {};

      var instruction = {
        viewModel: viewModel,
        childContainer: childContainer,
        view: config.view || config.viewStrategy,
        router: router
      };

      childContainer.registerSingleton(_routerView.RouterViewLocator);

      childContainer.getChildRouter = function () {
        var childRouter = void 0;

        childContainer.registerHandler(_aureliaRouter.Router, function (c) {
          return childRouter || (childRouter = router.createChild(childContainer));
        });

        return childContainer.get(_aureliaRouter.Router);
      };

      return this.compositionEngine.ensureViewModel(instruction);
    };

    return TemplatingRouteLoader;
  }(_aureliaRouter.RouteLoader)) || _class2);


  function createDynamicClass(moduleId) {
    var _dec3, _dec4, _class3;

    var name = /([^\/^\?]+)\.html/i.exec(moduleId)[1];

    var DynamicClass = (_dec3 = (0, _aureliaTemplating.customElement)(name), _dec4 = (0, _aureliaTemplating.useView)(moduleId), _dec3(_class3 = _dec4(_class3 = function () {
      function DynamicClass() {
        
      }

      DynamicClass.prototype.bind = function bind(bindingContext) {
        this.$parent = bindingContext;
      };

      return DynamicClass;
    }()) || _class3) || _class3);


    return DynamicClass;
  }
});