"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var companies_service_1 = require("../companies.service");
var router_1 = require("@angular/router");
var pagination_component_1 = require("../../layout/pagination/pagination.component");
var CompaniesListComponent = /** @class */ (function () {
    function CompaniesListComponent(service, route, router) {
        this.service = service;
        this.route = route;
        this.router = router;
        this.filter = { title: null };
        this.data = {};
        this.page = new pagination_component_1.Pagination();
    }
    CompaniesListComponent.prototype.ngOnInit = function () {
        //this.data = { "totalCount": 5, "items": [{ "id": 1, "title": "Company 1", "isApproved": true }, { "id": 2, "title": "Company 2", "isApproved": true }, { "id": 3, "title": "Company 3", "isApproved": true }, { "id": 4, "title": "Company e5beb3b5-d45c-4e1e-9787-27ff213b8508", "isApproved": true }, { "id": 5, "title": "jgfjgj2", "isApproved": false }] };
        var _this = this;
        this.route.queryParamMap.subscribe(function (prm) {
            for (var _i = 0, _a = prm.keys; _i < _a.length; _i++) {
                var k = _a[_i];
                var val = prm.get(k);
                if (!_this.page.parseRouteParam(k, val))
                    _this.filter[k] = val;
            }
            var p = Object.assign({}, _this.filter, _this.page.listParams());
            _this.service.getList(p)
                .subscribe(function (result) {
                Object.assign(_this.data, result);
                _this.page.setTotal(result.total);
            });
        });
    };
    CompaniesListComponent.prototype.applySearch = function () {
        var f = {};
        var names = Object.getOwnPropertyNames(this.filter);
        for (var _i = 0, names_1 = names; _i < names_1.length; _i++) {
            var n = names_1[_i];
            if (this.filter[n])
                f[n] = this.filter[n];
        }
        Object.assign(f, this.page.getUrlParams());
        this.router
            .navigate([this.route.routeConfig.path], {
            queryParams: f
        });
    };
    CompaniesListComponent.prototype.filterChanged = function () {
        var _this = this;
        var fltrJson = JSON.stringify(this.filter);
        if (fltrJson == this.prevFltrJson)
            return;
        this.prevFltrJson = fltrJson;
        if (this.searchTimeout) {
            clearTimeout(this.searchTimeout);
            this.searchTimeout = null;
        }
        this.searchTimeout = setTimeout(function () {
            _this.searchTimeout = null;
            _this.applySearch();
        }, 800);
    };
    CompaniesListComponent.prototype.clearTitle = function () {
        this.filter.title = '';
        this.filterChanged();
    };
    CompaniesListComponent = __decorate([
        core_1.Component({
            selector: 'app-companies-list',
            templateUrl: './companies-list.component.html',
            styleUrls: ['./companies-list.component.scss']
        }),
        __metadata("design:paramtypes", [companies_service_1.CompaniesService,
            router_1.ActivatedRoute,
            router_1.Router])
    ], CompaniesListComponent);
    return CompaniesListComponent;
}());
exports.CompaniesListComponent = CompaniesListComponent;
//# sourceMappingURL=companies-list.component.js.map