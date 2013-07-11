import requests 
from datetime import datetime
import re

class Alvo:

	def __init__(self, row = None):
		self.retornoRequisicao = 'vazio'

		if row is not None:
			self.row = row
			self.cdSite = row['site_cd_site']
			self.cdAlvo = row['alvo_cd_alvo']
			self.retornoRequisicao = row['retornoRequisicao']
			self.linkVisitado = row['linkVisitado']
			self.historicoStatus = row['historicoStatus']
			self.ultimaVisita = row['ultimaVisita']

	def getRow(self):
		self.row['site_cd_site'] = self.cdSite
		self.row['retornoRequisicao'] = self.retornoRequisicao
		self.row['linkVisitado'] = self.linkVisitado
		self.row['historicoStatus'] = self.historicoStatus
		self.row['ultimaVisita'] = self.ultimaVisita
		return self.row

	def getPk(self):
		return self.cdSite + ' + id '

	def atualizarHtml(self):
		self.retornoRequisicao = requests.get(self.linkVisitado).text
		self.retornoRequisicao = self.retornoRequisicao \
			.replace('\r\n', '').replace('\n', '').replace('\r', '') \
			.replace('"','').replace("'",'').replace('&acirc;','â') \
			.replace('ó', 'o').replace('á','a').replace('é','e') \
			.replace('í','i').replace('ú','u').replace('ã','a') \
			.replace('â','a').replace('&nbsp;', ' ').replace('^','') \
			.replace(',','')

		self.historicoStatus += 'r'
		self.ultimaVisita = str(datetime.now())

class Anuncio:
	def __init__(self, row = None):
		self.bairro = ''
		if row is not None:
			self.row = row
			self.cdSite = row['site_cd_site']
			self.cdAlvo = row['alvo_cd_alvo']
			self.bairro = row['bairro']
	
	def getRow(self):
		self.row['site_cd_site'] = self.cdSite
		self.row['alvo_cd_alvo'] = self.cdAlvo
		self.row['bairro'] = self.bairro
		return self.row


class Site:
	def __init__(self, row = None):
		if row is not None:
			self.row = row
			self.cdSite = row['site_cd_site']
			self.nmSite = row['site_nm_site']
			self.reBairro = row['regexBairro']
	
	def getRow(self):
		self.row['site_cd_site'] = self.cdSite
		self.row['site_nm_site'] = self.nmSite
		self.row['regexBairro'] = self.reBairro
		return self.row


	def getAnuncio(self, alvo):
		anuncio = Anuncio()
		anuncio.bairro = re.search(self.reBairro, alvo.retornoRequisicao).group(1)
		return anuncio
