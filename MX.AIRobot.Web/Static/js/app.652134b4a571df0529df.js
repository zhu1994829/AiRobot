webpackJsonp([6],{"G/pr":function(t,e){},NHnr:function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var a=n("7+uW"),o={name:"app-side",data:function(){return{active:""}},mounted:function(){this.active="/"+window.location.href.split("/")[4],console.log(this.$route.path)},methods:{handleOpen:function(t,e){console.log(t,e)},handleClose:function(t,e){console.log(t,e)}}},i={render:function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("el-menu",{staticClass:"el-menu-vertical-demo",attrs:{"default-active":t.active,router:"","background-color":"#344058","text-color":"#fff","active-text-color":"#ffd04b"},on:{open:t.handleOpen,close:t.handleClose}},[n("el-menu-item",{attrs:{index:"/"}},[n("i",{staticClass:"el-icon-star-off"}),t._v(" "),n("span",{attrs:{slot:"title"},slot:"title"},[t._v("首页")])]),t._v(" "),n("el-menu-item",{attrs:{index:"/problem"}},[n("i",{staticClass:"el-icon-menu"}),t._v(" "),n("span",{attrs:{slot:"title"},slot:"title"},[t._v("知识库")])]),t._v(" "),n("el-menu-item",{attrs:{index:"/rules"}},[n("i",{staticClass:"el-icon-setting"}),t._v(" "),n("span",{attrs:{slot:"title"},slot:"title"},[t._v("规则库")])])],1)},staticRenderFns:[]};var l={render:function(){var t=this.$createElement;return(this._self._c||t)("div",{staticClass:"logo"},[this._v("AI智能库管理中心")])},staticRenderFns:[]};var s={name:"app",components:{"app-side":n("VU/8")(o,i,!1,function(t){n("PKgC")},null,null).exports,"app-header":n("VU/8")({data:function(){return{tabActive1:0}}},l,!1,function(t){n("G/pr")},null,null).exports}},c={render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{attrs:{id:"app"}},[e("el-container",{staticStyle:{height:"100%"}},[e("el-header",{staticClass:"admin-header box-shadow"},[e("app-header")],1),this._v(" "),e("el-container",[e("el-aside",{staticClass:"admin-side",staticStyle:{width:"200px"}},[e("app-side")],1),this._v(" "),e("el-main",{staticClass:"admin-main"},[e("router-view")],1)],1)],1)],1)},staticRenderFns:[]},r=n("VU/8")(s,c,!1,null,null,null).exports,p=n("/ocq");a.default.use(p.a);var u=new p.a({routes:[{path:"/",name:"无答统计",component:function(t){n.e(3).then(function(){var e=[n("X0xP")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/problem",name:"知识库",component:function(t){n.e(4).then(function(){var e=[n("KbF4")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/problem/add",name:"添加知识库",component:function(t){n.e(1).then(function(){var e=[n("q4hR")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/problem/edit",name:"编辑知识库",component:function(t){n.e(1).then(function(){var e=[n("q4hR")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/rules",name:"规则库",component:function(t){n.e(2).then(function(){var e=[n("s81O")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/rules/add",name:"编辑规则",component:function(t){n.e(0).then(function(){var e=[n("tCsA")];t.apply(null,e)}.bind(this)).catch(n.oe)}},{path:"/rules/edit",name:"添加规则",component:function(t){n.e(0).then(function(){var e=[n("tCsA")];t.apply(null,e)}.bind(this)).catch(n.oe)}}]}),d=n("mtWM"),h=n.n(d),f=n("zL8q"),m=n.n(f),v=(n("tvR6"),n("sUu7"));n("rqsT");a.default.prototype.$http=h.a,a.default.use(m.a),a.default.use(v.a),a.default.config.productionTip=!1,a.default.prototype.httpUrl="http://localhost:60616",a.default.prototype.postHeader={"Content-Type":"application/x-www-form-urlencoded"},new a.default({el:"#app",router:u,components:{App:r},template:"<App/>"})},PKgC:function(t,e){},rqsT:function(t,e){},tvR6:function(t,e){}},["NHnr"]);
//# sourceMappingURL=app.652134b4a571df0529df.js.map