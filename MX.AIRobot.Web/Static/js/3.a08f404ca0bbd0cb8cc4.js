webpackJsonp([3],{"MWd/":function(t,e){},X0xP:function(t,e,i){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var n={render:function(){var t=this,e=t.$createElement,i=t._self._c||e;return i("div",{staticClass:"page"},[i("el-breadcrumb",{staticClass:"admin-breadcrumb",attrs:{"separator-class":"el-icon-arrow-right"}},[i("el-breadcrumb-item",{attrs:{to:{path:"/"}}},[t._v("首页")]),t._v(" "),i("el-breadcrumb-item",[t._v(t._s(t.title))])],1),t._v(" "),i("div",{staticClass:"headings"},[t._v(t._s(t.title))]),t._v(" "),i("div",{staticClass:"listBox"},[i("div",{staticClass:"listBtnBox",staticStyle:{float:"left"}},[i("el-button",{attrs:{type:"danger"},on:{click:t.del}},[t._v("删除")])],1),t._v(" "),i("el-form",{staticClass:"demo-form-inline",staticStyle:{float:"right",height:"55px"},attrs:{inline:!0}},[i("el-form-item",{attrs:{label:"问题"}},[i("el-input",{attrs:{placeholder:"问题关键字"},model:{value:t.keyword,callback:function(e){t.keyword=e},expression:"keyword"}})],1),t._v(" "),i("el-form-item",[i("el-button",{attrs:{type:"primary"},on:{click:t.getList}},[t._v("查询")])],1)],1),t._v(" "),i("el-table",{directives:[{name:"loading",rawName:"v-loading",value:t.loading,expression:"loading"}],ref:"listTable",staticStyle:{width:"100%"},attrs:{data:t.list.Items},on:{"selection-change":t.SelectionChange}},[i("el-table-column",{attrs:{type:"selection",width:"40"}}),t._v(" "),i("el-table-column",{attrs:{prop:"content",label:"问题"},scopedSlots:t._u([{key:"default",fn:function(e){return[i("div",{staticClass:"title",on:{click:function(i){t.to(e.$index,e.row)}}},[t._v(t._s(e.row.content))])]}}])}),t._v(" "),i("el-table-column",{attrs:{prop:"content",label:"提问次数",width:"200"}}),t._v(" "),i("el-table-column",{attrs:{fixed:"right",label:"操作",width:"100"},scopedSlots:t._u([{key:"default",fn:function(e){return[i("el-button",{attrs:{size:"mini",type:"primary"},on:{click:function(i){t.to(e.$index,e.row)}}},[t._v("设置答复")])]}}])})],1),t._v(" "),i("div",{staticClass:"listPage"},[i("el-pagination",{attrs:{background:"",layout:"total, prev, pager, next, jumper",total:t.list.TotalItems,"page-size":t.list.ItemsPerPage},on:{"current-change":t.handleCurrentChange}})],1)],1)],1)},staticRenderFns:[]};var a=i("VU/8")({data:function(){return{title:this.$route.name,loading:!0,keyword:"",list:{Items:[]},Selection:[]}},mounted:function(){this.getList()},methods:{handleCurrentChange:function(t){this.getList(t)},getList:function(t){var e=this;this.Selection=[],this.loading=!0,this.$http.get(this.httpUrl+"/Record/Index",{params:{page:t,Content:this.keyword}}).then(function(t){e.list=t.data,e.loading=!1})},to:function(t,e){this.$router.push({path:"/problem/add?countID="+this.list.Items[t].id+"&countTitle="+this.list.Items[t].content})},SelectionChange:function(t){this.Selection=[];for(var e=0;e<t.length;e++)this.Selection[e]=t[e].id},del:function(){var t=this;0!=this.Selection.length?this.$http.post(this.httpUrl+"/Record/Delete",this.Selection,{headers:this.postHeader}).then(function(e){"y"==e.data.status?(t.$notify({title:"成功",message:"删除成功",type:"success"}),t.getList()):t.$notify({title:"失败",message:e.data.info,type:"warning"})}):this.$notify({title:"警告",message:"您没有勾选任何选项！",type:"warning"})}}},n,!1,function(t){i("MWd/")},null,null);e.default=a.exports}});
//# sourceMappingURL=3.a08f404ca0bbd0cb8cc4.js.map