webpackJsonp([0],{YAKI:function(e,t){},tCsA:function(e,t,r){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var o={data:function(){return{title:this.$route.name,loading:!1,postLoading:!1,form:{keyone:"",keytwo:"",reply:"",type:"",url:"",id:""}}},mounted:function(){this.$route.query.id&&(this.id=this.$route.query.id,this.getData(this.id))},methods:{onSubmit:function(){var e=this;this.$refs.form.validate(function(t){if(!t)return console.log("error submit!!"),!1;e.postLoading=!0,e.$http.post(e.httpUrl+"/Rule/Edit",e.form,{headers:e.postHeader}).then(function(t){"y"==t.data.status?e.$message({message:"提交成功，返回列表",type:"success",duration:2e3,onClose:function(){window.history.go(-1)}}):(e.$message({message:t.data.info,type:"warning",duration:2e3}),e.postLoading=!1)})})},getData:function(e){var t=this;this.$http.get(this.httpUrl+"/Rule/Edit",{params:{id:e}}).then(function(e){t.form=e.data,t.loading=!1})}}},a={render:function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{staticClass:"page"},[r("el-breadcrumb",{staticClass:"admin-breadcrumb",attrs:{"separator-class":"el-icon-arrow-right"}},[r("el-breadcrumb-item",{attrs:{to:{path:"/"}}},[e._v("首页")]),e._v(" "),r("el-breadcrumb-item",{attrs:{to:{path:"/rules"}}},[e._v("规则库")]),e._v(" "),r("el-breadcrumb-item",[e._v(e._s(e.title))])],1),e._v(" "),r("div",{staticClass:"headings"},[e._v(e._s(e.title))]),e._v(" "),r("div",{staticClass:"admin-form"},[r("el-form",{ref:"form",attrs:{model:e.form,"label-width":"100px"}},[r("el-form-item",{attrs:{label:"前截取条件"}},[r("el-input",{model:{value:e.form.keyone,callback:function(t){e.$set(e.form,"keyone",t)},expression:"form.keyone"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"后截取条件"}},[r("el-input",{model:{value:e.form.keytwo,callback:function(t){e.$set(e.form,"keytwo",t)},expression:"form.keytwo"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"类型",prop:"type",rules:[{required:!0,message:"请选择类型",trigger:"change"}]}},[r("el-select",{attrs:{placeholder:"请选择类型"},model:{value:e.form.type,callback:function(t){e.$set(e.form,"type",t)},expression:"form.type"}},[r("el-option",{attrs:{label:"外部链接",value:"1"}}),e._v(" "),r("el-option",{attrs:{label:"内部连接",value:"2"}})],1)],1),e._v(" "),r("el-form-item",{attrs:{label:"跳转地址",prop:"url",rules:[{required:!0,message:"跳转地址不能为空",trigger:"blur"}]}},[r("el-input",{model:{value:e.form.url,callback:function(t){e.$set(e.form,"url",t)},expression:"form.url"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"回复内容"}},[r("el-input",{attrs:{type:"textarea"},model:{value:e.form.reply,callback:function(t){e.$set(e.form,"reply",t)},expression:"form.reply"}})],1),e._v(" "),r("el-form-item",[r("el-button",{attrs:{type:"primary"},on:{click:e.onSubmit}},[e._v("提交")]),e._v(" "),r("el-button",{attrs:{onclick:"history.go(-1)"}},[e._v("返回")])],1)],1)],1)],1)},staticRenderFns:[]};var l=r("VU/8")(o,a,!1,function(e){r("YAKI")},null,null);t.default=l.exports}});
//# sourceMappingURL=0.f82b6e3dfeb53abf6365.js.map