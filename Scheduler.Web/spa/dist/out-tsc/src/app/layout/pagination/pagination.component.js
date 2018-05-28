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
var PaginationComponent = /** @class */ (function () {
    function PaginationComponent() {
    }
    __decorate([
        core_1.Input(),
        __metadata("design:type", PaginationControlInfo)
    ], PaginationComponent.prototype, "control", void 0);
    PaginationComponent = __decorate([
        core_1.Component({
            selector: 'pagination',
            templateUrl: './pagination.component.html',
            styleUrls: ['./pagination.component.scss']
        })
    ], PaginationComponent);
    return PaginationComponent;
}());
exports.PaginationComponent = PaginationComponent;
var Pagination = /** @class */ (function () {
    function Pagination() {
        this.control = new PaginationControlInfo(0, 0, 0);
    }
    Pagination.prototype.listParams = function () {
        return new ListParams(this.getSkipCount(), this.rows, this.order || '', this.desc || false);
    };
    Pagination.prototype.parseRouteParam = function (k, val) {
        switch (k) {
            case "p.rows":
                this.rows = +val;
                return true;
            case "p.order":
                this.order = val;
                return true;
            case "p.desc":
                this.desc = val == "true";
                return true;
            default:
                return false;
        }
    };
    Pagination.prototype.getUrlParams = function () {
        var f = {};
        var names = ['page', 'rows', 'order', 'desc'];
        for (var _i = 0, names_1 = names; _i < names_1.length; _i++) {
            var n = names_1[_i];
            if (this[n])
                f['p.' + n] = this[n];
        }
        return f;
    };
    Pagination.prototype.getSkipCount = function () {
        return (this.page - 1) * this.rows;
    };
    Pagination.prototype.setTotal = function (total) {
        this.total = total;
        this.control = this.getControlInfo();
    };
    Pagination.prototype.getControlInfo = function () {
        var nums = 10;
        var skipCount = this.getSkipCount();
        var takeCount = skipCount + this.rows;
        if (takeCount > this.total)
            takeCount = this.total;
        var result = new PaginationControlInfo(skipCount + 1, takeCount, this.total);
        if (this.page < 1)
            this.page = 1;
        var d = this.total / this.rows;
        var totalPages = Math.ceil(d);
        if (totalPages > 1) {
            if (this.page > totalPages)
                this.page = totalPages;
            var firstPageNum = Math.floor(this.page / nums) * nums;
            var lastPageNum = firstPageNum + nums - 1;
            if (lastPageNum > totalPages - 1)
                lastPageNum = totalPages - 1;
            if (firstPageNum > 1)
                result.links.push(new PaginationLinkInfo(true, "<<", firstPageNum));
            if (this.page > 1)
                result.links.push(new PaginationLinkInfo(true, "<", this.page - 1));
            for (var i = firstPageNum; i <= lastPageNum; i++)
                result.links.push(new PaginationLinkInfo(this.page != i, i.toString(), i));
            if (this.page < totalPages)
                result.links.push(new PaginationLinkInfo(true, ">", this.page + 1));
            if (totalPages > nums && lastPageNum < totalPages)
                result.links.push(new PaginationLinkInfo(true, ">>", lastPageNum));
        }
        return result;
    };
    return Pagination;
}());
exports.Pagination = Pagination;
var ListParams = /** @class */ (function () {
    function ListParams(skipCount, takeCount, sortOrder, sortDesc) {
        this.skipCount = skipCount;
        this.takeCount = takeCount;
        this.sortOrder = sortOrder;
        this.sortDesc = sortDesc;
    }
    return ListParams;
}());
exports.ListParams = ListParams;
var PaginationControlInfo = /** @class */ (function () {
    function PaginationControlInfo(startIndex, endIndex, total) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.total = total;
    }
    return PaginationControlInfo;
}());
exports.PaginationControlInfo = PaginationControlInfo;
var PaginationLinkInfo = /** @class */ (function () {
    function PaginationLinkInfo(isLink, caption, page) {
        this.isLink = isLink;
        this.caption = caption;
        this.page = page;
    }
    return PaginationLinkInfo;
}());
exports.PaginationLinkInfo = PaginationLinkInfo;
//# sourceMappingURL=pagination.component.js.map