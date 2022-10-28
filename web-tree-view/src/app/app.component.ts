import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TreeView } from './tree-data/tree-data.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'web-tree-view';
  treeViewData: TreeView[] = [];
  constructor(private http: HttpClient) {

  }

  ngOnInit() {
    this.getTreeData();
  }

  getTreeData() {
    this.http.get<TreeView[]>('/assets/Items.json').subscribe(data => {
      this.treeViewData = data;
    });
  }
}
