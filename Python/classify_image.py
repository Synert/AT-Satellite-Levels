# Copyright 2017 The TensorFlow Authors. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ==============================================================================

#base code is from the tensorflow github
#https://github.com/tensorflow/tensorflow/blob/r1.6/tensorflow/examples/label_image/label_image.py
#modified to classify pictures in chunks, and print the results to files

from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import argparse
import sys

import numpy as np
import tensorflow as tf


def load_graph(model_file):
  graph = tf.Graph()
  graph_def = tf.GraphDef()

  with open(model_file, "rb") as f:
    graph_def.ParseFromString(f.read())
  with graph.as_default():
    tf.import_graph_def(graph_def)

  return graph


def read_tensor_from_image_file(file_name):
  input_height=299
  input_width=299
  input_mean=128
  input_std=128
  input_name = "file_reader"
  output_name = "normalized"
  file_reader = tf.read_file(file_name, input_name)
  if file_name.endswith(".png"):
    image_reader = tf.image.decode_png(
        file_reader, channels=3, name="png_reader")
  elif file_name.endswith(".gif"):
    image_reader = tf.squeeze(
        tf.image.decode_gif(file_reader, name="gif_reader"))
  elif file_name.endswith(".bmp"):
    image_reader = tf.image.decode_bmp(file_reader, name="bmp_reader")
  else:
    image_reader = tf.image.decode_jpeg(
        file_reader, channels=3, name="jpeg_reader")
  float_caster = tf.cast(image_reader, tf.float32)
  dims_expander = tf.expand_dims(float_caster, 0)
  resized = tf.image.resize_bilinear(dims_expander, [input_height, input_width])
  normalized = tf.divide(tf.subtract(resized, [input_mean]), [input_std])
  result = tf.Session().run(normalized)

  return result


def load_labels(label_file):
  label = []
  proto_as_ascii_lines = tf.gfile.GFile(label_file).readlines()
  for l in proto_as_ascii_lines:
    label.append(l.rstrip())
  return label


def classify(file_name):
  t = read_tensor_from_image_file(file_name)
  
  results = sess.run(output_operation.outputs[0], {
    input_operation.outputs[0]: t
  })
  
  sess.close()
  results = np.squeeze(results)

  top_k = results.argsort()[-6:][::-1]
  highest = 0
  highestFloor = 0
  secondHighestFloor = 0
  
  value = 0
  floorValue = 0
  otherValue = 0
  
  for i in top_k:
    results[i] *= 100
    if results[i] < 0.01:
      results[i] = 0
    if(results[i] > value):
      value = results[i]
      highest = i
    if (results[i] > floorValue and i != 0 and i != 4):
      floorValue = results[i]
      highestFloor = i
    if(results[i] > otherValue and i != 0 and i != 4 and results[i] < floorValue):
      otherValue = results[i]
      secondHighestFloor = i
  
  level_file.write(labels[highestFloor] + " " + labels[secondHighestFloor] + " " + str(floorValue) + " " + labels[highest] + " " + str(value) + "\n")
	
from PIL import Image
import os

os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3' 

def crop(Path, input, height, width, folder):
  im = Image.open(input)
  imgwidth, imgheight = im.size
  
  level_file.write(str(imgwidth / width) + " " + str(imgheight / height) + "\n")
  
  for i in range(0,imgheight,height):
    print ("{}%".format((i / imgheight) * 100))
    for j in range(0,imgwidth,width):
      box = (j, i, j+width, i+height)
      a = im.crop(box)
      a.save(os.path.join(Path,folder,"temp.jpg"))
      classify(os.path.join(Path,folder,"temp.jpg"))
      #tf.reset_default_graph()
  level_file.close()
  #im.close()
			
curDir = os.path.dirname(os.path.abspath(__file__))

curImage = "map3.jpg"
saveFolder = "map3"

model_file = os.path.join(curDir, "output_graph.pb")
label_file = os.path.join(curDir, "output_labels.txt")
input_layer = "Mul"
output_layer = "final_result"

graph = load_graph(model_file)
graph.finalize()
sess = tf.Session(graph=graph)
labels = load_labels(label_file)
os.makedirs(os.path.join(curDir, saveFolder), exist_ok=True)
level_file = open(os.path.join(curDir, saveFolder, "level.txt"), "w+")

input_name = "import/" + input_layer
output_name = "import/" + output_layer
input_operation = graph.get_operation_by_name(input_name)
output_operation = graph.get_operation_by_name(output_name)

#sess = tf.Session(graph=graph)

crop(curDir, os.path.join(curDir, curImage), 50, 50, saveFolder)
input('image successfully split, press ENTER to exit')