import { Component, Input, OnInit } from '@angular/core';
import { TreeView } from './tree-data.model';

@Component({
  selector: 'app-tree-data',
  templateUrl: './tree-data.component.html',
  styleUrls: ['./tree-data.component.scss']
})
export class TreeDataComponent implements OnInit {
  @Input() children: TreeView[] = [];
  constructor() { }

  ngOnInit(): void {
  }

}
