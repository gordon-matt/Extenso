/* */ 
define(['exports', 'aurelia-templating', 'aurelia-binding', 'aurelia-dependency-injection', 'aurelia-task-queue', 'aurelia-pal'], function (exports, _aureliaTemplating, _aureliaBinding, _aureliaDependencyInjection, _aureliaTaskQueue, _aureliaPal) {
  'use strict';

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.Focus = undefined;

  

  var _dec, _dec2, _class;

  var Focus = exports.Focus = (_dec = (0, _aureliaTemplating.customAttribute)('focus', _aureliaBinding.bindingMode.twoWay), _dec2 = (0, _aureliaDependencyInjection.inject)(_aureliaPal.DOM.Element, _aureliaTaskQueue.TaskQueue), _dec(_class = _dec2(_class = function () {
    function Focus(element, taskQueue) {
      

      this.element = element;
      this.taskQueue = taskQueue;
      this.isAttached = false;
      this.needsApply = false;
    }

    Focus.prototype.valueChanged = function valueChanged(newValue) {
      if (this.isAttached) {
        this._apply();
      } else {
        this.needsApply = true;
      }
    };

    Focus.prototype._apply = function _apply() {
      var _this = this;

      if (this.value) {
        this.taskQueue.queueMicroTask(function () {
          if (_this.value) {
            _this.element.focus();
          }
        });
      } else {
        this.element.blur();
      }
    };

    Focus.prototype.attached = function attached() {
      this.isAttached = true;
      if (this.needsApply) {
        this.needsApply = false;
        this._apply();
      }
      this.element.addEventListener('focus', this);
      this.element.addEventListener('blur', this);
    };

    Focus.prototype.detached = function detached() {
      this.isAttached = false;
      this.element.removeEventListener('focus', this);
      this.element.removeEventListener('blur', this);
    };

    Focus.prototype.handleEvent = function handleEvent(e) {
      if (e.type === 'focus') {
        this.value = true;
      } else if (_aureliaPal.DOM.activeElement !== this.element) {
        this.value = false;
      }
    };

    return Focus;
  }()) || _class) || _class);
});