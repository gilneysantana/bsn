import requests 
from datetime import datetime

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
		self.historicoStatus += 'r'
		self.ultimaVisita = str(datetime.now())
