/**
 * @license
 * Copyright 2023 Google LLC
 * SPDX-License-Identifier: Apache-2.0
 */

/*
This toolbox contains nearly every single built-in block that Blockly offers,
in addition to the custom block 'add_text' this sample app adds.
You probably don't need every single block, and should consider either rewriting
your toolbox from scratch, or carefully choosing whether you need each block
listed here.
*/
import { main_tools1 } from './main_tools';
import { task_1_1_tools } from './task_1_1';

export const toolbox1 = {
  'kind': 'categoryToolbox',
  'contents': 
  [
    {
    'kind': 'category',
    'name': 'all_tools',
    'contents': main_tools1,
    },
    {
      'kind': 'category',
      'name': 'task1.1',
      'contents': task_1_1_tools,
    }
  ]
};
