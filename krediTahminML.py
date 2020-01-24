# -*- coding: utf-8 -*-
"""
Created on Mon Dec 16 20:08:17 2019

@author: akin_
"""

import pandas as pd
import warnings
warnings.filterwarnings('ignore')

data = pd.read_excel(r'D:\krediVeriseti.xlsx')

data.head()
data.shape
data.describe()

krediMiktari = data.iloc[:,:1]
yas = data.iloc[:,1:2]
aldigiKrediSayisi = data.iloc[:,3:4]
evDurumu = data.iloc[:,2:3]
telefonDurumu = data.iloc[:,4:5]
krediDurumu = data.iloc[:,5:]

from sklearn.preprocessing import OneHotEncoder

ohe = OneHotEncoder(categorical_features='all')

OHEevDurumu = ohe.fit_transform(evDurumu).toarray()
OHEtelefonDurumu = ohe.fit_transform(telefonDurumu).toarray()
OHEkrediDurumu = ohe.fit_transform(krediDurumu).toarray()

sonucEvDurumu = pd.DataFrame(data = OHEevDurumu[:,:1], index = range(len(evDurumu)), columns=['evDurumu'])
sonucTelefonDurumu = pd.DataFrame(data = OHEtelefonDurumu[:,:1], index = range(len(telefonDurumu)), columns=['telefonDurumu'])
sonucKrediDurumu = pd.DataFrame(data = OHEkrediDurumu[:,:1], index = range(len(krediDurumu)), columns=['krediDurumu'])

dataSonuc = pd.concat([yas, aldigiKrediSayisi, krediMiktari, sonucEvDurumu, sonucTelefonDurumu], axis = 1)

from sklearn.model_selection import train_test_split

X_train, X_test, y_train, y_test = train_test_split(dataSonuc, sonucKrediDurumu, random_state=63, test_size=0.22)


from sklearn.neighbors import KNeighborsClassifier
knn = KNeighborsClassifier(n_neighbors=21, metric='chebyshev')

history = knn.fit(X_train, y_train)

pred = knn.predict(X_test)

from sklearn.metrics import accuracy_score

print(accuracy_score(y_test,pred))




import pickle
filename = r'D:\model.sav'
pickle.dump(history, open(filename, 'wb'))

#############################   JSON AÇ, VERİLERİ PREDICTIONA HAZIRLA
import flask
from flask import request
app = flask.Flask(__name__)
HOST = '0.0.0.0'
PORT = 4000
##
#import json
##j = {"yas":"", 
##     "aldigiKrediSayisi":"", 
##     "krediMiktari":"", 
##     "sonucEvDurumu":"", 
##     "sonucTelefonDurumu":""
##     }
#
#print(j)
#print("----------------------------------")
#j = pd.DataFrame(data = j, index = range(len(j)))
#j = pd.DataFrame(data = j, index = range(len(j.iloc[:1,:])))
#
#
#print(j)
#j_yas = j.iloc[:,:1]
#j_aldigiKrediSayisi = j.iloc[:,1:2]
#j_krediMiktari = j.iloc[:,2:3]
#j_evDurumu = j.iloc[:,3:4]
#j_telefonDurumu = j.iloc[:,4:]
#
#jSonuc = pd.concat([j_yas, j_aldigiKrediSayisi, j_krediMiktari, j_evDurumu, j_telefonDurumu], axis = 1)
#jSonuc['yas'] = pd.to_numeric(jSonuc['yas'],errors='coerce')
#jSonuc['aldigiKrediSayisi'] = pd.to_numeric(jSonuc['aldigiKrediSayisi'],errors='coerce')
#jSonuc['krediMiktari'] = pd.to_numeric(jSonuc['krediMiktari'],errors='coerce')
#jSonuc['sonucEvDurumu'] = pd.to_numeric(jSonuc['sonucEvDurumu'],errors='coerce')
#jSonuc['sonucTelefonDurumu'] = pd.to_numeric(jSonuc['sonucTelefonDurumu'],errors='coerce')
#
#j_scaler = scaler.fit_transform(jSonuc)
#
loaded_model = pickle.load(open(filename, 'rb'))
##result = loaded_model.predict(j_scaler)
##print(result)

@app.route('/krediTahmin/predict', methods=['POST'])
def predict():
    print(request.data)
    if (request.data):
        print("test 113")
        jdata = request.get_json()
        if (jdata['age']  ):
            print("116")
            print(str(jdata))
            age = jdata['age']
            print(age)
            creditCount  = jdata["creditCount"]
            creditSize= jdata["creditSize"]
            homeState = jdata["homeState"]
            phoneState = jdata["phoneState"]
           #dataSonuc = pd.concat([yas, aldigiKrediSayisi, krediMiktari, sonucEvDurumu, sonucTelefonDurumu], axis = 1)
            result =  loaded_model.predict([[int(age) , int(creditCount), float(creditSize) , int(homeState) , int(phoneState)]])
     
            
    return flask.jsonify(result[0])
#    age =  
#    crediCount = 
#    crediSize
#    homeState=
#    phone
     
     



#from sklearn.metrics import confusion_matrix
#print('\n conf matrix')
#print(confusion_matrix(y_test, result))

#from sklearn.metrics import accuracy_score

#print(accuracy_score(y_test,result))


if __name__ == "__main__":
    app.run(HOST, PORT, use_reloader=False, threaded=True, debug=True)







